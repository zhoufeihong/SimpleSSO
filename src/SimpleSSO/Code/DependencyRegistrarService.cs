using Autofac;
using FreeBird.Infrastructure.Core;
using SimpleSSO.Application.System;
using System.Linq;

namespace SimpleSSO.Code
{
    public partial class DependencyRegistrar
    {
        public void RegisterService(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            // 服务
            builder.RegisterAssemblyTypes(typeof(UserService).Assembly)
           .Where(t => t.Name.EndsWith("Service")).AsSelf().InstancePerRequest();
        }

    }
}