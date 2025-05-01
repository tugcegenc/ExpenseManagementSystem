using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Expense.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserRole
{
    Admin,
    Personnel
}
