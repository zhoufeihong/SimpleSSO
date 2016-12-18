using FreeBird.Infrastructure.Core;
using FreeBird.Infrastructure.OAuth;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Linq;

namespace SimpleSSO.Code.OAuth
{
    public class SimpleAuthenticationTokenProvider : AuthenticationTokenProvider
    {
        protected ITicketManage TicketManage => EngineContext.Current.Resolve<ITicketManage>();

        public string TokenType
        {
            get;
            set;
        }

        public TimeSpan ExpireTimeSpan { get; set; }

        public bool RemoveWhenReceive = false;

        /// <summary>
        /// 重新生成Token条件
        /// </summary>
        public Predicate<GrantData> OldTokenRemovetPredicate = data => true;

        /// <summary>
        /// 保留Token条件
        /// </summary>
        public Predicate<GrantData> TokenKeepingPredicate = data => false;

        public SimpleAuthenticationTokenProvider()
        {
        }

        private int GetExpiryMinutes()
        {
            if (ExpireTimeSpan != null)
            {
                return (int)ExpireTimeSpan.TotalMinutes;
            }
            return 1;
        }

        public override void Create(AuthenticationTokenCreateContext context)
        {
            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow.AddMinutes(GetExpiryMinutes());
            GrantData grantData = new GrantData
            {
                GrantType = context.OwinContext.Get<string>("as:grant_type"),
                ResponseType = context.OwinContext.Get<string>("as:response_type"),
                TokenType = TokenType,
                UserID = context.OwinContext.Get<string>("as:user_id"),
                UserName = context.OwinContext.Get<string>("as:user_name"),
                ClientID = context.OwinContext.Get<string>("as:client_id"),
                IssuedUtc = startDate,
                ExpiresUtc = endDate,
                RoleScope = GetIdentityRole(context.Ticket.Identity)
            };
            string tokenValue;
            if (TokenKeepingPredicate(grantData))
            {
                if (TicketManage.GetToken(grantData))
                {
                    context.SetToken(grantData.Token);
                    return;
                }
            }
            if (OldTokenRemovetPredicate(grantData))
            {
                TicketManage.RemoveOldTicket(grantData);
            }
            tokenValue = Convert.ToBase64String(Encoding.Default.GetBytes(Guid.NewGuid().ToString("n"))).TrimEnd('=').Replace('+', '-').Replace('/', '_');
            context.Ticket.Properties.IssuedUtc = startDate;
            context.Ticket.Properties.ExpiresUtc = endDate;
            grantData.Ticket = context.SerializeTicket();
            grantData.Token = tokenValue;
            TicketManage.SetTicketValue(grantData);
            context.SetToken(tokenValue);
        }

        public override void Receive(AuthenticationTokenReceiveContext context)
        {
            GrantData grantData = new GrantData()
            {
                Token = context.Token
            };
            if (RemoveWhenReceive)
            {
                TicketManage.RemoveTicketValue(ref grantData);
                context.DeserializeTicket(grantData.Ticket);
            }
            else
            {
                TicketManage.GetTicketValue(ref grantData);
                context.DeserializeTicket(grantData.Ticket);
            }
            context.OwinContext.Set("as:user_id", grantData.UserID);
            context.OwinContext.Set("as:user_name", grantData.UserName);
            context.OwinContext.Set("as:client_id", grantData.ClientID);
        }

        private static string GetIdentityRole(IIdentity identity)
        {
            ClaimsIdentity identity2 = identity as ClaimsIdentity;
            if (identity2 != null)
            {
                return identity2.FindFirst(identity2.RoleClaimType).Value;
            }
            return null;
        }

    }
}