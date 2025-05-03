namespace Expense.Application.Services.Interfaces.Services;

public interface IRabbitMqPublisher
{
    void Publish(string queueName, string message);
}