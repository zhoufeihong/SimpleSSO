using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FreeBird.Infrastructure.Domain.Repositories
{
    /// <summary>
    ///  代表一个资源库。
    /// </summary>
    /// <typeparam name="TEntity">实体类型。</typeparam>
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        /// <summary>
        /// 检查是否存在满足条件的实体。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <returns>如果存在满足条件的实体，返回true；否则返回false。</returns>
        bool Exists(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 获取指定属性的最大值。
        /// </summary>
        /// <typeparam name="TResult">属性类型。</typeparam>
        /// <param name="selector">属性选择表达式。</param>
        /// <returns>属性的最大值。</returns>
        TResult Max<TResult>(Expression<Func<TEntity, TResult>> selector);

        /// <summary>
        /// 获取指定属性的最大值。
        /// </summary>
        /// <typeparam name="TResult">属性类型。</typeparam>
        /// <param name="expression">条件表达式。</param>
        /// <param name="selector">属性选择表达式。</param>
        /// <returns>属性的最大值。</returns>
        TResult Max<TResult>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selector);

        /// <summary>
        /// 获取指定的实体。
        /// </summary>
        /// <param name="id">实体主键。</param>
        /// <returns>实体对象。</returns>
        TEntity Get(Guid id); 

        /// <summary>
        /// 获取指定的实体。
        /// </summary>
        /// <param name="expression">查询条件表达式。</param>
        /// <returns>实体对象。</returns>
        TEntity Get(Expression<Func<TEntity, bool>> expression); 

        /// <summary>
        /// 获取指定的实体。
        /// </summary>
        /// <typeparam name="TProperty">关联查询属性。</typeparam>
        /// <param name="path">关联查询路径。</param>
        /// <param name="expression">查询条件表达式。</param>
        /// <returns>实体对象。</returns>
        TEntity Get<TProperty>(Expression<Func<TEntity, TProperty>> path, Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 获取指定的实体。
        /// </summary>
        /// <param name="paths">关联查询路径列表</param>
        /// <param name="expression">查询条件表达式。</param>
        /// <returns>实体对象。</returns>
        TEntity Get(IList<string> paths, Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 获取所有实体。
        /// </summary>
        /// <returns>实体列表。</returns>
        IList<TEntity> GetAll();

        /// <summary>
        /// 获取所有实体。
        /// </summary>
        /// <typeparam name="TProperty">关联查询属性。</typeparam>
        /// <param name="path">关联查询路径。</param>
        /// <returns>实体列表。</returns>
        IList<TEntity> GetAll<TProperty>(Expression<Func<TEntity, TProperty>> path);

        /// <summary>
        /// 获取所有实体。
        /// </summary>
        /// <param name="paths">关联查询路径列表。</param>
        /// <returns>实体列表。</returns>
        IList<TEntity> GetAll(IList<string> paths);

        /// <summary>
        /// 根据指定的条件查询实体。
        /// </summary>
        /// <param name="expression">查询条件表达式。</param>
        /// <returns>符合条件的实体列表。</returns>
        IList<TEntity> Query(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 根据指定的条件查询实体。
        /// </summary>
        /// <typeparam name="TProperty">关联查询属性。</typeparam>
        /// <typeparam name="TKey">排序属性。</typeparam>
        /// <param name="expression">查询条件表达式。</param>
        /// <param name="keySelector">排序属性选择器。</param>
        /// <param name="isAscending">是否按升序排序。</param>
        /// <returns>符合条件的实体列表。</returns>
        IList<TEntity> Query<TKey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TKey>> keySelector, bool isAscending);

        /// <summary>
        /// 根据指定的条件查询实体。
        /// </summary>
        /// <typeparam name="TProperty">关联查询属性。</typeparam>
        /// <param name="path">关联查询路径。</param>
        /// <param name="expression">查询条件表达式。</param>
        /// <returns>符合条件的实体列表。</returns>
        IList<TEntity> Query<TProperty>(Expression<Func<TEntity, TProperty>> path, Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 根据指定的条件查询实体。
        /// </summary>
        /// <param name="paths">关联查询路径列表。</param>
        /// <param name="expression">查询条件表达式。</param>
        /// <returns>符合条件的实体列表。</returns>
        IList<TEntity> Query(IList<string> paths, Expression<Func<TEntity, bool>> expression);

        Task<IList<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 根据指定的条件查询实体。
        /// </summary>
        /// <typeparam name="TProperty">关联查询属性。</typeparam>
        /// <typeparam name="TKey">排序属性。</typeparam>
        /// <param name="path">关联查询路径。</param>
        /// <param name="expression">查询条件表达式。</param>
        /// <param name="keySelector">排序属性选择器。</param>
        /// <param name="isAscending">是否按升序排序。</param>
        /// <returns>符合条件的实体列表。</returns>
        IList<TEntity> Query<TProperty, TKey>(Expression<Func<TEntity, TProperty>> path, Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TKey>> keySelector, bool isAscending);

        /// <summary>
        /// 根据指定的条件查询实体。
        /// </summary>
        /// <typeparam name="TKey">排序属性。</typeparam>
        /// <param name="paths">关联查询路径列表。</param>
        /// <param name="expression">查询条件表达式。</param>
        /// <param name="keySelector">排序属性选择器。</param>
        /// <param name="isAscending">是否按升序排序。</param>
        /// <returns>符合条件的实体列表。</returns>
        IList<TEntity> Query<TKey>(IList<string> paths, Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TKey>> keySelector, bool isAscending);
        /// <summary>
        /// 根据指定的条件查询实体。
        /// </summary>
        /// <typeparam name="TKey">排序属性。</typeparam>
        /// <param name="expression">查询条件表达式。</param>
        /// <param name="keySelector">排序属性选择器。</param>
        /// <param name="isAscending">是否按升序排序。</param>
        /// <param name="pageParam">分页信息</param>
        /// <returns>符合条件的实体列表。</returns>
        IList<TEntity> Query<TKey>(Expression<Func<TEntity, bool>> expression,
            Expression<Func<TEntity, TKey>> keySelector, bool isAscending, IPageParam pageParam);

        /// <summary>
        /// 根据指定的条件查询实体。
        /// </summary>
        /// <typeparam name="TProperty">关联查询属性。</typeparam>
        /// <typeparam name="TKey">排序属性。</typeparam>
        /// <param name="path">关联查询路径。</param>
        /// <param name="expression">查询条件表达式。</param>
        /// <param name="keySelector">排序属性选择器。</param>
        /// <param name="isAscending">是否按升序排序。</param>
        /// <param name="pageParam">分页信息</param>
        /// <returns>符合条件的实体列表。</returns>
        IList<TEntity> Query<TProperty, TKey>(Expression<Func<TEntity, TProperty>> path, Expression<Func<TEntity, bool>> expression,
            Expression<Func<TEntity, TKey>> keySelector, bool isAscending, IPageParam pageParam);

        /// <summary>
        /// 根据指定的条件查询实体。
        /// </summary>
        /// <typeparam name="TKey">排序属性。</typeparam>
        /// <param name="paths">关联查询路径列表</param>
        /// <param name="expression">查询条件表达式。</param>
        /// <param name="keySelector">排序属性选择器。</param>
        /// <param name="isAscending">是否按升序排序。</param>
        /// <param name="pageParam">分页信息</param>
        /// <returns>符合条件的实体列表。</returns>
        IList<TEntity> Query<TKey>(IList<string> paths, Expression<Func<TEntity, bool>> expression,
           Expression<Func<TEntity, TKey>> keySelector, bool isAscending, IPageParam pageParam);

        /// <summary>
        /// 新增实体。
        /// </summary>
        /// <param name="entity">实体对象。</param>
        void Add(TEntity entity);

        /// <summary>
        /// 更新实体。
        /// </summary>
        /// <param name="entity">实体对象。</param>
        void Update(TEntity entity);

        /// <summary>
        /// 删除实体。
        /// </summary>
        /// <param name="id">实体主键。</param>
        void Delete(Guid id);

        /// <summary>
        /// 删除实体。
        /// </summary>
        /// <param name="entity">实体对象。</param>
        void Delete(TEntity entity);

        /// <summary>
        /// 按需新增实体（调用后即时提交到数据库，无需调用UnitOfWork.Commit）。
        /// </summary>
        /// <param name="entity">实体对象。</param>
        void AddOnDemand(TEntity entity);

        /// <summary>
        /// 按需更新实体（调用后即时提交到数据库，无需调用UnitOfWork.Commit）。
        /// </summary>
        /// <param name="entity">实体对象。</param>
        void UpdateOnDemand(TEntity entity);

        /// <summary>
        /// 按需更新实体（调用后即时提交到数据库，无需调用UnitOfWork.Commit）。
        /// </summary>
        /// <param name="id">实体ID。</param>
        /// <param name="updateExpression">用于标识需更新属性的表达式。</param>
        void UpdateOnDemand(Guid id, Expression<Func<TEntity, TEntity>> updateExpression);

        /// <summary>
        /// 按需更新实体（调用后即时提交到数据库，无需调用UnitOfWork.Commit）。
        /// </summary>
        /// <param name="expression">用于查询需更新实体的表达式。</param>
        /// <param name="updateExpression">用于标识需更新属性的表达式。</param>
        void UpdateOnDemand(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TEntity>> updateExpression);

        /// <summary>
        /// 按需删除实体（调用后即时提交到数据库，无需调用UnitOfWork.Commit）。
        /// </summary>
        /// <param name="id">实体主键。</param>
        void DeleteOnDemand(Guid id);

        /// <summary>
        /// 按需删除实体（调用后即时提交到数据库，无需调用UnitOfWork.Commit）。
        /// </summary>
        /// <param name="ids">实体ID列表。</param>
        void DeleteOnDemand(IEnumerable<Guid> ids);

        /// <summary>
        /// 按需删除实体（调用后即时提交到数据库，无需调用UnitOfWork.Commit）。
        /// </summary>
        /// <param name="expression">用于查询需删除实体的表达式。</param>
        void DeleteOnDemand(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 按需删除实体（调用后即时提交到数据库，无需调用UnitOfWork.Commit）。
        /// </summary>
        /// <param name="entity">实体对象。</param>
        void DeleteOnDemand(TEntity entity);
    }
}
