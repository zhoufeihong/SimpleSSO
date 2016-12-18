using FreeBird.Infrastructure.Core;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.OAuth;
using SimpleSSO.Results;
using System.Web.Http;
using static SimpleSSO.Setting;

namespace SimpleSSO.Controllers
{
    [RoutePrefix("GrantCode")]
    public class GrantCodeController : ApiController
    {
        [Route("Authorize")]
        [AllowAnonymous]
        [HttpGet]
        public IHttpActionResult Authorize()
        {
            var OAuthOptions = EngineContext.Current.Resolve<OAuthAuthorizationServerOptions>();
            if (User.Identity == null || User.Identity.IsAuthenticated == false)
            {
                return new ChallengeResult(DefaultAuthenticationTypes.ApplicationCookie, this);
            }
            return Redirect(Url.Content("~/") + EndPointConfig.AuthorizeGrant + Request.RequestUri.Query);
        }

    }
}
