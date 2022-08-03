using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GenericRepositoryEFCore
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> Create(TEntity entity);
        Task<TEntity?> Find(int id, params Expression<Func<TEntity, object>>[] includeExpressions);
        Task<TEntity?> Find(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeExpressions);
        Task<IEnumerable<TEntity>> FindAll(params Expression<Func<TEntity, object>>[] includeExpressions);
        Task<IEnumerable<TEntity>> FindAll(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeExpressions);
        Task<TEntity?> Update(TEntity entity);
        Task<bool> Delete(int id);
    }
}
