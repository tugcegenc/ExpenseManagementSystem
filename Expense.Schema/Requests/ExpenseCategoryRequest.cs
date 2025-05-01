namespace Expense.Schema.Requests;

public class CreateExpenseCategoryRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public class UpdateExpenseCategoryRequest
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}