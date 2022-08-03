using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GenericRepositoryEFCore
{
    public class GenericRepository<TEntity> : BaseRepository, IRepository<TEntity> where TEntity : class, new()
    {
        private readonly Type typeEntity;
        public GenericRepository(DbContext applicationDbContext) : base(applicationDbContext)
        {
            typeEntity = typeof(TEntity);
            if (typeEntity.GetProperty("Id") == null)
            {
                throw new Exception("The entity don't have an Id property");
            }
        }

        public virtual async Task<TEntity> Create(TEntity entity)
        {
            TEntity addedEntity = (await _db.Set<TEntity>().AddAsync(entity)).Entity;
            await _db.SaveChangesAsync();

            return addedEntity;
        }

        private DbSet<TEntity> GetDBSetWithIncludes(Expression<Func<TEntity, object>>[]? includeExpressions = null)
        {
            if (includeExpressions == null)
                return _db.Set<TEntity>();
            return (DbSet<TEntity>) includeExpressions.Aggregate<Expression<Func<TEntity, object>>, IQueryable<TEntity>>(_db.Set<TEntity>(), (current, expression) => current.Include(expression));
        }

        public virtual async Task<TEntity?> Find(int id, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            return await GetDBSetWithIncludes(includeExpressions).FindAsync(id);
        }

        public virtual async Task<TEntity?> Find(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            return await GetDBSetWithIncludes(includeExpressions).FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<IEnumerable<TEntity>> FindAll(params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            return await GetDBSetWithIncludes(includeExpressions).ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> FindAll(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            return await GetDBSetWithIncludes(includeExpressions).Where(predicate).ToListAsync();
        }

        public virtual async Task<TEntity?> Update(TEntity entity)
        {
            if (entity == null)
                return null;
            TEntity? entityFromDb = await _db.Set<TEntity>().FindAsync(typeEntity.GetProperty("Id")!.GetValue(entity));
            if (entityFromDb == null)
                return null;
            foreach (var property in typeEntity.GetProperties())
            {
                var valueFromDb = property.GetValue(entityFromDb);
                var valueNewEntity = property.GetValue(entity);
                if (property.Name != "Id" && property.CanWrite && valueFromDb != valueNewEntity)
                    property.SetValue(entityFromDb, valueNewEntity);
            }
            await _db.SaveChangesAsync();
            return await _db.Set<TEntity>().FindAsync(typeEntity.GetProperty("Id")!.GetValue(entity));
        }

        public virtual async Task<bool> Delete(int id)
        {
            TEntity? entity = await _db.Set<TEntity>().FindAsync(id);
            if(entity != null)
                _db.Set<TEntity>().Remove(entity);
            return await _db.SaveChangesAsync() == 1;
        }
    }
}
