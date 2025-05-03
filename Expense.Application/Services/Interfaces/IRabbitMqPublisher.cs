namespace Expense.Application.Services.Interfaces;

public interface IRabbitMqPublisher
{
    void Publish(string queueName, string message);
}