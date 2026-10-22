using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Repositories
{
    /// <summary>
    /// Общий контракт репозитория для доступа к данным.
    /// Контроллеры и сервисы не обращаются к DbContext напрямую — только через репозитории.
    /// </summary>
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes);

        Task<IReadOnlyList<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            int? skip = null,
            int? take = null,
            params Expression<Func<TEntity, object>>[] includes);

        Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null);

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Remove(TEntity entity);

        /// <summary>
        /// Единственная точка сохранения: все изменения, накопленные репозиториями
        /// (в том числе в бизнес-операциях над несколькими сущностями),
        /// записываются в БД одним вызовом SaveChangesAsync.
        /// </summary>
        Task SaveChangesAsync();
    }
}
