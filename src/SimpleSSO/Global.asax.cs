using FreeBird.Infrastructure.Core;
using FreeBird.Infrastructure.TypeUtilities.TypeAdapter;
using FreeBird.Infrastructure.TypeUtilities.TypeAdapter.AutoMapper;
using SimpleSSO.Application.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SimpleSSO
{
    public class SimpleSSOApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //类型转换（automap）
            TypeAdapterFactory.SetCurrent(new AutomapperTypeAdapterFactory());
            //初始化IOC
            EngineContext.Initialize(false);
        }
    }
}
