using Expense.Application.Services.Interfaces.Repositories;

namespace Expense.Application.Services.Interfaces.Infrastucture;

public interface IUnitOfWork
{
    IGenericRepository<T> GetRepository<T>() where T : class;
    Task<long> CompleteAsync();
    void Dispose();
}
