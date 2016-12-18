using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace FreeBird.Infrastructure.Core.Authorize
{
    public interface IFreeBirdAuthorize
    {
        bool IsAuthorized(HttpActionContext actionContext,string name);
        bool IsAuthorized(AuthorizationContext filterContext, string name);
    }
}
