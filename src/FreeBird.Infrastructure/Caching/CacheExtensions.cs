using Autofac;
using Autofac.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBird.Infrastructure.Caching
{
    public static class ContainerManagerExtensions
    {
        public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> WithStaticCache<TLimit, TReflectionActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration) where TReflectionActivatorData : ReflectionActivatorData
        {
            return registration.WithParameter(Autofac.Core.ResolvedParameter.ForNamed<ICacheManager>("static"));
        }

        public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> WithRequestCache<TLimit, TReflectionActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration) where TReflectionActivatorData : ReflectionActivatorData
        {
            return registration.WithParameter(Autofac.Core.ResolvedParameter.ForNamed<ICacheManager>("request"));
        }

    }
}
