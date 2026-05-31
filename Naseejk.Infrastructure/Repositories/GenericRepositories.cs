using Microsoft.EntityFrameworkCore;
using Naseejk.core.Entities;
using Naseejk.core.Repositories;
using Naseejk.core.Specifications;
using Naseejk.Infrastructure.Data;
using Naseejk.core.Interfaces;
using System.Linq.Expressions;

namespace Naseejk.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IBaseEntity
    {
        private readonly NaseejkDbContext _context;

        public GenericRepository(NaseejkDbContext context)
        {
            _context = context;
        }

        // 🟢 تطبيق الـ Id كـ long
        public async Task<T?> GetByIdAsync(long id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        // 🟢 تطبيق الدالة المطلوبة في الـ Interface
        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetEntityWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<bool> ExistsAsync(long id)
        {
            return await _context.Set<T>().AnyAsync(x => x.Id == id);
        }
    }
}