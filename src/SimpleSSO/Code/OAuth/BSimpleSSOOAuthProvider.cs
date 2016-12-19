using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using FreeBird.Infrastructure.Core;
using SimpleSSO.Application.System;
using FreeBird.Infrastructure.OAuth;
using FreeBird.Infrastructure.Exceptions;
using SimpleSSO.Domain.System;
using static SimpleSSO.Setting;
using SimpleSSO.SignalRHubs.Admin;
using Microsoft.AspNet.SignalR;
using SimpleSSO.SignalRHubs;

namespace SimpleSSO.Code.OAuth
{
    public class SimpleSSOOAuthProvider : OAuthAuthorizationServerProvider
    {
        private AppService _appService => EngineContext.Current.Resolve<AppService>();

        private IHubContext _adminMessageHub = GlobalHost.ConnectionManager.GetHubContext<AdminMessageHub>();

        private SendMessageService _sendMessageService = new SendMessageService();

        protected ITicketStore TicketStore => EngineContext.Current.Resolve<ITicketStore>();

        private readonly string _asClientID = "as:client_id";

        private readonly string _asUserID = "as:user_id";

        private readonly string _asUserName = "as:user_name";

        private readonly string _asGrantType = "as:grant_type";

        private readonly string _asResponseType = "as:response_type";

        private readonly string _asClientData = "as:client_data";

        private readonly string _scopeAll = "user_all";

        private readonly string _clientIDErro = "client_id不存在.";

        private readonly string _clientAppErro = "client_id不存在应用 .";

        public SimpleSSOOAuthProvider()
        {

        }

        /// <summary>
        /// Password
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Set(_asGrantType, GrantTypes.Password);
            var userService = EngineContext.Current.Resolve<UserService>();
            var result = userService.Login(context.UserName, context.Password);
            if (result.Success)
            {
                var user = result.Data;
                ClaimsIdentity oAuthIdentity = ClaimsIdentityCreate.GenerateUserIdentity(user, OAuthDefaults.AuthenticationType);
                _sendMessageService.SendToAdmin(_adminMessageHub, $"AppID为{context.ClientId}客户端，给用户{context.UserName}申请Password授权成功.");
                //设置角色
                oAuthIdentity.AddRole(RoleConfig.AppUserAllRole);
                AuthenticationProperties properties = CreateProperties(user.Name);
                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
                //设置上下文
                context.OwinContext.Set(_asClientID, context.ClientId);
                context.OwinContext.Set(_asUserID, user.UserID.ToString());
                context.OwinContext.Set(_asUserName, context.UserName);
            }
            else
            {
                context.SetError(result.Message);
            }
            return base.GrantResourceOwnerCredentials(context);
        }

        /// <summary>
        /// ClientCredentials
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            var oAuthIdentity = ClaimsIdentityCreate.GenerateAppIdentity(context.ClientId, "", OAuthDefaults.AuthenticationType);
            //设置角色
            oAuthIdentity.AddClaim(new Claim(oAuthIdentity.RoleClaimType, RoleConfig.AppRole));
            var ticket = new AuthenticationTicket(oAuthIdentity, new AuthenticationProperties());
            context.Validated(ticket);
            //设置上下文
            context.OwinContext.Set(_asGrantType, GrantTypes.ClientCredentials);
            context.OwinContext.Set(_asClientID, context.ClientId);

            return base.GrantClientCredentials(context);
        }

        /// <summary>
        /// RefreshToken
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            context.OwinContext.Set(_asGrantType, GrantTypes.RefreshToken);
            context.OwinContext.Set(_asClientID, context.ClientId);
            var newId = new ClaimsIdentity(context.Ticket.Identity);
            var role = newId.FindFirstValue(newId.RoleClaimType);
            if (role != null)
            {
                if (role.Split(',').All(c => !c.Equals(RoleConfig.AppRole, StringComparison.OrdinalIgnoreCase)))
                {
                    context.OwinContext.Set(_asUserID, newId.GetUserId());
                }
            }
            var newTicket = new AuthenticationTicket(newId, context.Ticket.Properties);
            context.Validated(newTicket);
            return base.GrantRefreshToken(context);
        }

        /// <summary>
        /// AuthorizationCode
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantAuthorizationCode(OAuthGrantAuthorizationCodeContext context)
        {
            context.OwinContext.Set(_asGrantType, GrantTypes.AuthorizationCode);
            return base.GrantAuthorizationCode(context);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// AuthorizationCode and ImplicitGrantType
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task AuthorizeEndpoint(OAuthAuthorizeEndpointContext context)
        {
            var owinContext = context.OwinContext;
            context.OwinContext.Set(_asResponseType, context.AuthorizeRequest.ResponseType);
            if (context.Request.User?.Identity != null && context.Request.User.Identity.IsAuthenticated)
            {
                var userService = EngineContext.Current.Resolve<UserService>();
                var user = userService.GetUserByName((context.Request.User.Identity.Name));
                if (user == null)
                {
                    return;
                }
                var clientID = context.AuthorizeRequest.ClientId;
                var isTemporaryAuthorization = TicketStore.Remove("TemporaryAuthorization" +
                    clientID
                    + "$" + user.UserID.ToString());
                //可信应用不需要用户授权
                if (!isTemporaryAuthorization && !context.OwinContext.Get<App>(_asClientData).IsCredible)
                {
                    return;
                }
                ClaimsIdentity oAuthIdentity = ClaimsIdentityCreate.GenerateUserIdentity(user, OAuthDefaults.AuthenticationType);
                if (context.AuthorizeRequest.Scope.Any(t => t.Equals(_scopeAll, StringComparison.OrdinalIgnoreCase)))
                    oAuthIdentity.AddRole(RoleConfig.AppUserAllRole);
                else
                    oAuthIdentity.AddRole(RoleConfig.AppUserBaseRole);

                context.OwinContext.Set(_asClientID, clientID);
                context.OwinContext.Set(_asUserID, user.UserID.ToString());
                context.OwinContext.Set(_asUserName, user.Name);

                AuthenticationProperties properties = CreateProperties(user.Name);
                properties.RedirectUri = context.AuthorizeRequest.RedirectUri;
                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                owinContext.Authentication.SignIn(oAuthIdentity);
                context.RequestCompleted();
            }
            await base.AuthorizeEndpoint(context);
        }

        public override Task AuthorizationEndpointResponse(OAuthAuthorizationEndpointResponseContext context)
        {
            return base.AuthorizationEndpointResponse(context);
        }

        public override Task TokenEndpointResponse(OAuthTokenEndpointResponseContext context)
        {
            return base.TokenEndpointResponse(context);
        }

        public override Task MatchEndpoint(OAuthMatchEndpointContext context)
        {
            return base.MatchEndpoint(context);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret;
            if (context.TryGetFormCredentials(out clientId, out clientSecret)
                || context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                var clientIDStr = clientId;
                int clientID;
                if (string.IsNullOrEmpty(clientIDStr) || !int.TryParse(clientIDStr, out clientID))
                {
                    context.SetError(_clientIDErro);
                    return base.ValidateClientAuthentication(context);
                }
                var app = _appService.Query(new DTO.System.AppDTO
                {
                    ClientID = clientID
                }, null).ToList().FirstOrDefault();
                if (app == null)
                {
                    context.SetError(_clientAppErro);
                    return base.ValidateClientAuthentication(context);
                }
                if (app.ClientSecret == clientSecret)
                {
                    context.Validated(clientId);
                    context.OwinContext.Set(_asClientData, app);
                }
            }
            return base.ValidateClientAuthentication(context);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            var clientIDStr = context.ClientId;
            int clientID;
            if (string.IsNullOrEmpty(clientIDStr) || !int.TryParse(clientIDStr, out clientID))
            {
                throw new BusinessException(_clientIDErro);
            }
            var app = _appService.Query(new DTO.System.AppDTO
            {
                ClientID = clientID
            }, null).ToList().FirstOrDefault();
            if (app == null)
            {
                throw new BusinessException(_clientAppErro);
            }
            if (context.RedirectUri.Equals(app.ReturnUrl, StringComparison.OrdinalIgnoreCase))
            {
                context.Validated(context.RedirectUri);
            }
            context.OwinContext.Set(_asClientData, app);
            return Task.FromResult<object>(null);
        }

        public override Task ValidateAuthorizeRequest(OAuthValidateAuthorizeRequestContext context)
        {
            return base.ValidateAuthorizeRequest(context);
        }

        public override Task ValidateTokenRequest(OAuthValidateTokenRequestContext context)
        {
            context.Validated();
            return base.ValidateTokenRequest(context);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }

    }
}