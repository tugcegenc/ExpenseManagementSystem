namespace Expense.Common.RabbitMQ;

public class EmailMessage
{
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}