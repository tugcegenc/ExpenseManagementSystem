using System.Text.Json.Serialization;

namespace Expense.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))] 
public enum PeriodType
{ 
        Daily,
        Weekly,
        Monthly
}

