using Naseejk.core.Entities;
using Naseejk.core.Repositories;
using Naseejk.core.Interfaces;
using Naseejk.Infrastructure.Data;
using Naseejk.Infrastructure.Repositories;
using System.Collections;

namespace Naseejk.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NaseejkDbContext _context;
        private Hashtable? _repositories;

        public UnitOfWork(NaseejkDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<T> Repository<T>() where T : class, IBaseEntity
        {
            // Check if the repositories hashtable is null, if so, initialize it store repositories for different entity types. The hashtable will use the name of the entity type as the key and destroy it when the request ends.
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }
            var type = typeof(T).Name;

            // Check if the repository for the specified entity type already exists in the hashtable. If it does not exist, create a new instance of the GenericRepository for that entity type and add it to the hashtable.
            if (!_repositories.ContainsKey(type))
            {
                // Get the type of the generic repository, which is GenericRepository<>. This is a generic type definition that can be used to create instances of the generic repository for different entity types.
                var repositoryType = typeof(GenericRepository<>);
                // Dynamic Type Creation
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
                // make instance of the generic repository for the specified entity type and pass the database context to its constructor. This allows the repository to perform database operations using the provided context.
                _repositories.Add(type, repositoryInstance);
            }
            return (IGenericRepository<T>)_repositories[type]!;
        }
            public async Task<int> CompleteAsync()
            {
                return await _context.SaveChangesAsync();
            }
            public void Dispose()
            {
                // Dispose the database context when the unit of work is disposed. This ensures that all resources used by the context are released properly.
                _context.Dispose();
            }

            public async Task BeginTransactionAsync() => await _context.Database.BeginTransactionAsync();
            public async Task CommitAsync() => await _context.Database.CommitTransactionAsync();
            public async Task RollbackAsync() => await _context.Database.RollbackTransactionAsync();
    }
    
}
