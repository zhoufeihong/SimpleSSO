using SimpleSSOTest.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using Microsoft.Owin.Security.OAuth;
using Microsoft.AspNet.Identity;
using System.Net.Http;

namespace SimpleSSOTest.Controllers
{
    [RoutePrefix("login")]
    public class LoginController : ApiController
    {
        private string _provider = "SimpleSSOAuthentication";

        private const string ExternalCookie = "ExternalCookie";

        [HttpGet]
        [Route("authsimplesso")]
        public IHttpActionResult AuthSimpleSSO()
        {
            var indentity = Authentication.GetExternalIdentity(ExternalCookie);
            if (indentity == null)
            {
                return new ChallengeResult(_provider, this);
            }
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(indentity as ClaimsIdentity);
            if (externalLogin == null)
            {
                return InternalServerError();
            }

            //注销凭证
            Authentication.SignOut(ExternalCookie);

            //登录逻辑
            //...

            return Json($"测试应用通过{externalLogin.LoginProvider}获取到用户:{externalLogin.UserName}信息");
        }

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string UserID { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, UserID, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                var idClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (idClaim == null || String.IsNullOrEmpty(idClaim.Issuer)
                    || String.IsNullOrEmpty(idClaim.Value))
                {
                    return null;
                }

                if (idClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = idClaim.Issuer,
                    UserID = idClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

    }
}