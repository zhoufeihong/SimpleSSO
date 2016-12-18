using FreeBird.Infrastructure.Exceptions;
using FreeBird.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace FreeBird.Infrastructure.Core.Authorize
{
    public abstract class FreeBirdAuthorize : IFreeBirdAuthorize
    {
        protected FreeBirdAuthorizeModel CreateModel(HttpActionContext actionContext, string name)
        {
            var attributes = actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<FreeBirdApiAuthorizeAttribute>(true);
            if (attributes == null || attributes.Count <= 0)
            {
                throw new AuthorizationException();
            }
            string nodeName = string.IsNullOrEmpty(attributes[0].Name) ?
             actionContext.ActionDescriptor.ControllerDescriptor.ControllerName : attributes[0].Name;

            string actionName = string.IsNullOrEmpty(name) ?
                actionContext.ActionDescriptor.ActionName : name;

            var identity = actionContext.ControllerContext.RequestContext.Principal.Identity;

            string userID = GetIdentityValue(identity,
                ClaimTypes.NameIdentifier);
            return new FreeBirdAuthorizeModel
            {
                UserID = int.Parse(userID),
                UserName = identity.Name,
                PrincipalRole = GetIdentityRole(identity),
                NodeName = nodeName,
                ActionName = actionName,
                NodeRole = attributes[0].Roles
            };
        }

        protected FreeBirdAuthorizeModel CreateModel(AuthorizationContext filterContext, string name)
        {
            var attributes = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(FreeBirdMvcAuthorizeAttribute), true);
            if (attributes == null || attributes.Length <= 0)
            {
                throw new AuthorizationException();
            }
            var attribute = attributes[0] as FreeBirdMvcAuthorizeAttribute;
            string nodeName = string.IsNullOrEmpty(attribute.Name) ?
             filterContext.ActionDescriptor.ControllerDescriptor.ControllerName : attribute.Name;

            string actionName = string.IsNullOrEmpty(name) ?
                filterContext.ActionDescriptor.ActionName : name;

            var identity = filterContext.HttpContext.User.Identity;

            string userID = GetIdentityValue(identity,
                ClaimTypes.NameIdentifier);
            return new FreeBirdAuthorizeModel
            {
                UserID = int.Parse(userID),
                UserName = identity.Name,
                PrincipalRole = GetIdentityRole(identity),
                NodeName = nodeName,
                ActionName = actionName,
                NodeRole = attribute.Roles
            };
        }


        public bool IsAuthorized(HttpActionContext actionContext, string name)
        {
            return IsAuthorized(CreateModel(actionContext, name));
        }

        public bool IsAuthorized(AuthorizationContext filterContext, string name)
        {
            return IsAuthorized(CreateModel(filterContext, name));
        }

        /// <summary>
        /// 应用权限验证
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected abstract bool IsAuthorized(FreeBirdAuthorizeModel model);

        private static string GetIdentityValue(IIdentity identity, string valueType)
        {
            Guard.ArgumentNotNull(identity, nameof(identity));
            ClaimsIdentity identity2 = identity as ClaimsIdentity;
            if (identity2 != null)
            {
                return identity2.FindFirst(valueType).Value;
            }
            return null;
        }

        private static string GetIdentityRole(IIdentity identity)
        {
            Guard.ArgumentNotNull(identity, nameof(identity));
            ClaimsIdentity identity2 = identity as ClaimsIdentity;
            if (identity2 != null)
            {
                return identity2.FindFirst(identity2.RoleClaimType).Value;
            }
            return null;
        }
    }
}
