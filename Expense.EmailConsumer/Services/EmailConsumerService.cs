using System.Net.Mail;
using System.Text;
using System.Text.Json;
using Expense.Common.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Expense.EmailConsumer.Services;

public class EmailConsumerService : IEmailConsumerService
{
    private readonly ConnectionFactory _connectionFactory;

    public EmailConsumerService()
    {
        _connectionFactory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };
    }

    public void Start()
    {
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        var queueName = "email-queue";

        channel.QueueDeclare(queue: queueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var messageJson = Encoding.UTF8.GetString(body);

            try
            {
                var emailMessage = JsonSerializer.Deserialize<EmailMessage>(messageJson);

                if (emailMessage == null || string.IsNullOrWhiteSpace(emailMessage.To))
                {
                    Console.WriteLine("Invalid email message received.");
                    return;
                }

                await SendEmailAsync(emailMessage);

                Console.WriteLine($"Email sent to: {emailMessage.To}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        };

        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

        Console.ReadLine();
    }

    private async Task SendEmailAsync(EmailMessage message)
    {
        var mail = new MailMessage
        {
            From = new MailAddress("expensemanagementsystemm@gmail.com"),
            Subject = message.Subject,
            Body = message.Body
        };
        mail.To.Add(message.To);

        using var smtp = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new System.Net.NetworkCredential("expensemanagementsystemm@gmail.com", "zikq zoif chrd shbn"),
            EnableSsl = true
        };

        await smtp.SendMailAsync(mail);
    }
}
