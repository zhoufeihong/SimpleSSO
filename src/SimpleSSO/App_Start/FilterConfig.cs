using FreeBird.Infrastructure.Mvc;
using Microsoft.AspNet.Identity;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace SimpleSSO
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new FreeBirdHandleErrorAttribute());
 
        }
    }
}
