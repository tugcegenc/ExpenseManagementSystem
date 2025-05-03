using Expense.Application.Services.Interfaces.Infrastucture;
using Expense.Application.Services.Interfaces.Repositories;
using Expense.Infrastructure.Context;
using Expense.Infrastructure.Repositories;

namespace Expense.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ExpenseDbContext _dbcontext;
    private readonly Dictionary<Type, object> _repositories;

    public UnitOfWork(ExpenseDbContext dbcontext)
    {
        _dbcontext = dbcontext;
        _repositories = new Dictionary<Type, object>();
    }

    public IGenericRepository<T> GetRepository<T>() where T : class
    {
        var type = typeof(T);

        if (!_repositories.ContainsKey(type))
        {
            var repoInstance = new GenericRepository<T>(_dbcontext);
            _repositories[type] = repoInstance;
        }

        return (IGenericRepository<T>)_repositories[type];
    }

    public async Task<long> CompleteAsync()
    {
        using var transaction = await _dbcontext.Database.BeginTransactionAsync();
        try
        {
            var result = await _dbcontext.SaveChangesAsync();
            await transaction.CommitAsync();
            return result;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public void Dispose()
    {
        _dbcontext.Dispose();
    }

}


