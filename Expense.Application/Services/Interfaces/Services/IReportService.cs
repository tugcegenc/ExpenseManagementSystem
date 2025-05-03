using Expense.Common.ApiResponse;
using Expense.Schema.Responses;

namespace Expense.Application.Services.Interfaces.Services;

public interface IReportService
{
    Task<List<PersonnelExpenseResponse>> GetPersonnelExpensesAsync(long userId);

}
