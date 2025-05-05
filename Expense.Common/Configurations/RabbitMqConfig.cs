namespace Expense.Common.Configurations;

public class RabbitMqConfig
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string QueueName { get; set; }
}
