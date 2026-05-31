using Naseejk.core.Entities;
using Naseejk.core.Specifications;
using System.Linq.Expressions;
using Naseejk.core.Interfaces;

namespace Naseejk.core.Repositories
{
    public interface IGenericRepository<T> where T : class, IBaseEntity
    {
        Task<T?> GetByIdAsync(long id);

        Task<IReadOnlyList<T>> ListAllAsync();

        Task<T?> GetEntityWithSpec(ISpecification<T> spec);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        Task<IReadOnlyList<T>> ListAllAsync(Expression<Func<T, bool>> predicate);

        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);

        Task<bool> ExistsAsync(long id);
    }
}