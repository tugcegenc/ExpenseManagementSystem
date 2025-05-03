using Expense.Application.Services.Interfaces.Services;
using Expense.Common.ApiResponse;
using Expense.Schema.Responses;

namespace Expense.Application.Services.Implementations;

public class ReportService : IReportService
{
    public Task<List<PersonnelExpenseResponse>> GetPersonnelExpensesAsync(long userId)
    {
        throw new NotImplementedException();
    }
}

