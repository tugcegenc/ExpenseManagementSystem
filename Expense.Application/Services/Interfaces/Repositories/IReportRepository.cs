using Expense.Schema.Responses;

namespace Expense.Application.Services.Interfaces.Repositories;

public interface IReportRepository
{
    Task<List<PersonnelExpenseResponse>> GetPersonnelExpensesAsync(long userId);
}