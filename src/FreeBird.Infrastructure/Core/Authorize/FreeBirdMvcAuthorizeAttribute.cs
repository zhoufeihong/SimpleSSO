using FreeBird.Infrastructure.Core;
using FreeBird.Infrastructure.Core.Authorize;
using FreeBird.Infrastructure.Exceptions;
using FreeBird.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FreeBird.Infrastructure.Core.Authorize
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class FreeBirdMvcAuthorizeAttribute : AuthorizeAttribute
    {
        public string Name { get; set; }

        private IFreeBirdAuthorize _freeBirdAuthorize => EngineContext.Current.Resolve<IFreeBirdAuthorize>();

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            Guard.ArgumentNotNull(filterContext, nameof(filterContext));
            if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
            {
                throw new InvalidOperationException("AuthorizeAttribute can not use within Child action cache");
            }
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                HandleUnauthorizedRequest(filterContext);
                return;
            }
            if (!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) && !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                if (this.AuthorizeCore(filterContext.HttpContext))
                {
                    HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
                    cache.SetProxyMaxAge(new TimeSpan(0L));
                    cache.AddValidationCallback(new HttpCacheValidateHandler(this.CacheValidateHandler), null);

                }
                else
                {
                    throw new AuthorizationException();
                }
            }
            if (!_freeBirdAuthorize.IsAuthorized(filterContext, Name))
            {
                throw new AuthorizationException();
            }
        }

        private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = this.OnCacheAuthorization(new HttpContextWrapper(context));
        }


        private static string GetUserId(System.Security.Principal.IIdentity identity)
        {
            Guard.ArgumentNotNull(identity, nameof(identity));
            ClaimsIdentity identity2 = identity as ClaimsIdentity;
            if (identity2 != null)
            {
                return identity2.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            return null;
        }

    }
}
