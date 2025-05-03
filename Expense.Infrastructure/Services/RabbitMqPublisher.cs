using System.Text;
using Expense.Application.Services.Interfaces.Services;
using RabbitMQ.Client;

namespace Expense.Infrastructure.Services;

public class RabbitMqPublisher : IRabbitMqPublisher
{
     private readonly IConnectionFactory _connectionFactory;

    public RabbitMqPublisher(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    public void Publish(string queueName, string message)
    {
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: queueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "",
                             routingKey: queueName,
                             basicProperties: null,
                             body: body);
    }
}