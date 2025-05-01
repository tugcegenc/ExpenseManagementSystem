using Expense.Domain.Interfaces.Repositories;

namespace Expense.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> GetRepository<T>() where T : class;
        Task<long> CompleteAsync();
        void Dispose();
    }
}
