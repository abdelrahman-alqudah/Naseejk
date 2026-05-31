using Naseejk.core.Entities;
using Naseejk.core.Repositories;
using Naseejk.core.Interfaces;
namespace Naseejk.core.Repositories
{
    public interface IUnitOfWork:IDisposable
    {
        IGenericRepository<T> Repository<T> () where T : class ,IBaseEntity;


        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task<int> CompleteAsync();

    }
}
