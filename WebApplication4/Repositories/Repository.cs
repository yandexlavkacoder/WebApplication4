using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Repositories
{
    /// <summary>
    /// Базовый репозиторий поверх DbContext. Все репозитории конкретных сущностей
    /// наследуются от него. SaveChangesAsync здесь единственный — поэтому бизнес-операция,
    /// меняющая несколько сущностей через разные репозитории одного scope, сохраняется атомарно.
    /// </summary>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ShopDbContext Context;
        protected readonly DbSet<TEntity> Set;

        public Repository(ShopDbContext context)
        {
            Context = context;
            Set = context.Set<TEntity>();
        }

        public virtual async Task<TEntity?> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = Set;
            foreach (var include in includes)
                query = query.Include(include);

            // Единая конвенция: PK сущности называется <Имя>Id.
            var parameter = Expression.Parameter(typeof(TEntity), "e");
            var property = Expression.Property(parameter, typeof(TEntity).Name + "Id");
            var idEquals = Expression.Equal(property, Expression.Constant(id));
            var predicate = Expression.Lambda<Func<TEntity, bool>>(idEquals, parameter);

            return await query.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            int? skip = null,
            int? take = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = Set;

            foreach (var include in includes)
                query = query.Include(include);

            if (filter is not null)
                query = query.Where(filter);

            if (orderBy is not null)
                query = orderBy(query);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);

            return await query.ToListAsync();
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            IQueryable<TEntity> query = Set;
            if (filter is not null)
                query = query.Where(filter);
            return await query.CountAsync();
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await Set.AddAsync(entity);
        }

        public virtual void Update(TEntity entity)
        {
            Set.Update(entity);
        }

        public virtual void Remove(TEntity entity)
        {
            Set.Remove(entity);
        }

        public Task SaveChangesAsync()
        {
            return Context.SaveChangesAsync();
        }
    }
}
