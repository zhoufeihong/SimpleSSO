using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.AspNet.Identity;

[assembly: OwinStartup(typeof(SimpleSSO.Startup))]

namespace SimpleSSO
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR("/signalr", new HubConfiguration());
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new MyUserIdProvider());
        }

        class MyUserIdProvider : IUserIdProvider
        {
            private PrincipalUserIdProvider _principalUserIdProvider = new PrincipalUserIdProvider();

            public string GetUserId(IRequest request)
            {
                string userID = request.User.Identity.GetUserId();
                if (!string.IsNullOrEmpty(userID))
                {
                    return userID;
                }
                return _principalUserIdProvider.GetUserId(request);
            }
        }
    }
}
