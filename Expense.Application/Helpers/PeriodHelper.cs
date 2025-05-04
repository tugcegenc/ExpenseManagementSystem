using Expense.Domain.Enums;

namespace Expense.Infrastructure.Helpers;
public static class PeriodHelper
{
    public static (DateTime startDate, DateTime endDate) GetPeriodRange(PeriodType periodType)
    {
        var today = DateTime.Now.Date;

        return periodType switch
        {
            PeriodType.Daily =>
            (
                today,
                today.AddDays(1).AddTicks(-1)
            ),

            PeriodType.Weekly =>
            (
                today.AddDays(-((int)(today.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)today.DayOfWeek - 1))),
                today.AddDays(7 - ((int)(today.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)today.DayOfWeek - 1))).AddTicks(-1)
            ),

            PeriodType.Monthly =>
            (
                new DateTime(today.Year, today.Month, 1),
                new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month)).AddDays(1).AddTicks(-1)
            ),

            _ => (today, today.AddDays(1).AddTicks(-1))
        };
    }
}