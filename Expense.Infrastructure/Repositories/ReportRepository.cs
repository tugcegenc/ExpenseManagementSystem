using Expense.Application.Services.Interfaces.Repositories;
using Expense.Schema.Responses;


namespace Expense.Infrastructure.Repositories;

public class ReportRepository : IReportRepository
{
    public Task<List<PersonnelExpenseResponse>> GetPersonnelExpensesAsync(long userId)
    {
        throw new NotImplementedException();
    }

}
