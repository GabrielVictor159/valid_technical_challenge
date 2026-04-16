namespace Store.Application.Abstractions.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<T> Repository<T>() where T : class;
    Task<int> CommitAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
