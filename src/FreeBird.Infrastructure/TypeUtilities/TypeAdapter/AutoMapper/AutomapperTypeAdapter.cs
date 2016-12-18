using System.Collections.Generic;
using AutoMapper;

namespace FreeBird.Infrastructure.TypeUtilities.TypeAdapter.AutoMapper
{
    /// <summary>
    /// Automapper type adapter implementation
    /// </summary>
    public class AutomapperTypeAdapter
        : ITypeAdapter
    {
        #region ITypeAdapter Members


        public TTarget Adapt<TSource, TTarget>(TSource source)
            where TSource : class, new()
            where TTarget : class, new()
        {
            return Mapper.Map<TSource, TTarget>(source);
        }

        public TTarget Adapt<TSource, TTarget>(TSource source, TTarget target)
            where TSource : class, new()
            where TTarget : class, new()
        {
            return Mapper.Map(source, target);
        }

        public IEnumerable<TTarget> Adapt<TSource, TTarget>(IEnumerable<TSource> source)
            where TSource : class, new()
            where TTarget : class, new()
        {
            return Mapper.Map<IEnumerable<TSource>, IEnumerable<TTarget>>(source);
        }

        public TTarget Adapt<TTarget>(object source) where TTarget : class, new()
        {
            return Mapper.Map<TTarget>(source);
        }

        #endregion
    }
}
