using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using SimpleSSO.Domain;
using FreeBird.Infrastructure;
using EntityFramework.Extensions;
using FreeBird.Infrastructure.Domain;
using FreeBird.Infrastructure.Domain.Uow;
using FreeBird.Infrastructure.Domain.Repositories;
using FreeBird.Infrastructure.Utilities;
using FreeBird.Infrastructure.Linq;
using System.Threading.Tasks;

namespace SimpleSSO.EFRepositories
{
    /// <summary>
    /// 资源库基类。
    /// </summary>
    /// <typeparam name="TEntity">实体类型。</typeparam>
    public class EFRepository<TEntity> : IRepository<TEntity>, IUnitOfWorkRepository where TEntity : EntityBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbContext _dbContext;

        public EFRepository(IUnitOfWork unitOfWork, DbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
        }

        protected IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        protected DbContext DbContext
        {
            get { return _dbContext; }
        }

        protected DbSet<TEntity> Entities
        {
            get { return _dbContext.Set<TEntity>(); }
        }

        public virtual bool Exists(Expression<Func<TEntity, bool>> expression)
        {
            return Entities.Any(expression);
        }

        public virtual TResult Max<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            return Entities.Max(selector);
        }

        public virtual TResult Max<TResult>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selector)
        {
            return Entities.Where(expression).Max(selector);
        }

        public virtual TEntity Get(Guid id)
        {
            return Entities.Find(id);
        }

        public virtual TEntity Get(Expression<Func<TEntity, bool>> expression)
        {
            return Entities.FirstOrDefault(expression);
        }

        public virtual TEntity Get<TProperty>(Expression<Func<TEntity, TProperty>> path, Expression<Func<TEntity, bool>> expression)
        {
            return Entities.Include(path).FirstOrDefault(expression);
        }

        public virtual TEntity Get(IList<string> paths, Expression<Func<TEntity, bool>> expression)
        {
            DbQuery<TEntity> entities = Entities;
            if (paths != null)
            {
                paths.ToList().ForEach(p => entities = entities.Include(p));
            }

            return entities.FirstOrDefault(expression);
        }

        public virtual IList<TEntity> GetAll()
        {
            return Entities.ToList();
        }

        public virtual IList<TEntity> GetAll<TProperty>(Expression<Func<TEntity, TProperty>> path)
        {
            return Entities.Include(path).ToList();
        }

        public virtual IList<TEntity> GetAll(IList<string> paths)
        {
            DbQuery<TEntity> entities = Entities;
            if (paths != null)
            {
                paths.ToList().ForEach(p => entities = entities.Include(p));
            }

            return entities.ToList();
        }

        public virtual IList<TEntity> Query(Expression<Func<TEntity, bool>> expression)
        {
            return Entities.AsExpandable().Where(expression).ToList();
        }

        public async virtual Task<IList<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Entities.AsExpandable().Where(expression).ToListAsync();
        }

        public virtual IList<TEntity> Query<TKey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TKey>> keySelector, bool isAscending)
        {
            var entities = Entities.AsExpandable().Where(expression);
            if (isAscending)
            {
                return entities.OrderBy(keySelector).ToList();
            }
            return entities.OrderByDescending(keySelector).ToList();
        }

        public virtual IList<TEntity> Query<TProperty>(Expression<Func<TEntity, TProperty>> path, Expression<Func<TEntity, bool>> expression)
        {
            return Entities.Include(path).AsExpandable().Where(expression).ToList();
        }

        public virtual IList<TEntity> Query(IList<string> paths, Expression<Func<TEntity, bool>> expression)
        {
            DbQuery<TEntity> entities = Entities;
            if (paths != null)
            {
                paths.ToList().ForEach(p => entities = entities.Include(p));
            }

            return entities.AsExpandable().Where(expression).ToList();
        }

        public virtual IList<TEntity> Query<TProperty, TKey>(Expression<Func<TEntity, TProperty>> path, Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TKey>> keySelector, bool isAscending)
        {
            var entities = Entities.Include(path).AsExpandable().Where(expression);
            if (isAscending)
            {
                return entities.OrderBy(keySelector).ToList();
            }
            return entities.OrderByDescending(keySelector).ToList();
        }

        public virtual IList<TEntity> Query<TKey>(IList<string> paths, Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TKey>> keySelector, bool isAscending)
        {
            DbQuery<TEntity> entities = Entities;

            if (paths != null)
            {
                paths.ToList().ForEach(p => entities = entities.Include(p));
            }

            var query = entities.AsExpandable().Where(expression);

            if (isAscending)
            {
                return query.OrderBy(keySelector).ToList();
            }
            return query.OrderByDescending(keySelector).ToList();
        }

        public virtual IList<TEntity> Query<TKey>(Expression<Func<TEntity, bool>> expression,
            Expression<Func<TEntity, TKey>> keySelector, bool isAscending, IPageParam pageParam)
        {
            Guard.ArgumentNotNull(pageParam, "pageParam");
            pageParam.TotalRecordCount = 0;
            var query = Entities.AsExpandable().Where(expression);

            if (isAscending)
            {
                query = query.OrderBy(keySelector);
            }
            else
            {
                query = query.OrderByDescending(keySelector);
            }

            if (pageParam.Limit > 0) //分页查询
            {
                pageParam.Offset--;
                if (pageParam.Offset < 0)
                {
                    pageParam.Offset = 0;
                }
                pageParam.TotalRecordCount = query.Count();
                query = query.Skip(pageParam.Offset).Take(pageParam.Limit);
            }

            return query.ToList();
        }
        public virtual IList<TEntity> Query<TProperty, TKey>(Expression<Func<TEntity, TProperty>> path, Expression<Func<TEntity, bool>> expression,
            Expression<Func<TEntity, TKey>> keySelector, bool isAscending, IPageParam pageParam)
        {
            Guard.ArgumentNotNull(pageParam, "pageParam");
            pageParam.TotalRecordCount = 0;
            var query = Entities.Include(path).AsExpandable().Where(expression);

            if (isAscending)
            {
                query = query.OrderBy(keySelector);
            }
            else
            {
                query = query.OrderByDescending(keySelector);
            }

            if (pageParam.Limit > 0) //分页查询
            {
                pageParam.Offset--;
                if (pageParam.Offset < 0)
                {
                    pageParam.Offset = 0;
                }
                pageParam.TotalRecordCount = query.Count();
                query = query.Skip(pageParam.Offset).Take(pageParam.Limit);
            }

            return query.ToList();
        }

        public virtual IList<TEntity> Query<TKey>(IList<string> paths, Expression<Func<TEntity, bool>> expression,
            Expression<Func<TEntity, TKey>> keySelector, bool isAscending, IPageParam pageParam)
        {
            Guard.ArgumentNotNull(pageParam, "pageParam");
            pageParam.TotalRecordCount = 0;

            DbQuery<TEntity> entities = Entities;
            if (paths != null)
            {
                paths.ToList().ForEach(p => entities = entities.Include(p));
            }

            var query = entities.AsExpandable().Where(expression);
            if (isAscending)
            {
                query = query.OrderBy(keySelector);
            }
            else
            {
                query = query.OrderByDescending(keySelector);
            }

            if (pageParam.Limit > 0) //分页查询
            {
                pageParam.Offset--;
                if (pageParam.Offset < 0)
                {
                    pageParam.Offset = 0;
                }
                pageParam.TotalRecordCount = query.Count();
                query = query.Skip(pageParam.Offset).Take(pageParam.Limit);
            }

            return query.ToList();
        }

        public virtual void Add(TEntity entity)
        {
            _unitOfWork.RegisterAdded(entity, this);
        }

        public virtual void Update(TEntity entity)
        {
            _unitOfWork.RegisterChangeded(entity, this);
        }

        public virtual void Delete(Guid id)
        {
            TEntity entity = Get(id);
            if (entity != null)
            {
                _unitOfWork.RegisterRemoved(entity, this);
            }
        }

        public virtual void Delete(TEntity entity)
        {
            _unitOfWork.RegisterRemoved(entity, this);
        }

        public virtual void AddOnDemand(TEntity entity)
        {
            Entities.Add(entity);
            _dbContext.SaveChanges();
        }

        public virtual void UpdateOnDemand(TEntity entity)
        {
            _dbContext.Entry<TEntity>(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public virtual void UpdateOnDemand(Guid id, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            Entities.Where(e => e.ID == id).Update(updateExpression);
        }

        public virtual void UpdateOnDemand(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            Entities.Where(expression).Update(updateExpression);
        }

        public virtual void DeleteOnDemand(Guid id)
        {
            Entities.Where(e => e.ID == id).Delete();
        }

        public virtual void DeleteOnDemand(IEnumerable<Guid> ids)
        {
            Entities.Where(e => ids.Contains(e.ID)).Delete();
        }

        public virtual void DeleteOnDemand(Expression<Func<TEntity, bool>> expression)
        {
            Entities.Where(expression).Delete();
        }

        public virtual void DeleteOnDemand(TEntity entity)
        {
            _dbContext.Entry<TEntity>(entity).State = EntityState.Deleted;
            _dbContext.SaveChanges();
        }

        public virtual void PersistNewItem(EntityBase entityBase)
        {
            //由EF负责持久化
        }

        public virtual void PersistUpdatedItem(EntityBase entityBase)
        {
            //由EF负责持久化
        }

        public virtual void PersistDeletedItem(EntityBase entityBase)
        {
            //由EF负责持久化
        }
    }
}
