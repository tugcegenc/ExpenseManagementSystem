using System.Net.Mail;
using System.Text;
using System.Text.Json;
using Expense.Common.Configurations;
using Expense.Common.RabbitMQ;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Expense.EmailConsumer.Services;

public class EmailConsumerService : IEmailConsumerService
{
    private readonly IConfiguration _configuration;
    private readonly RabbitMqConfig _rabbitMqSettings;
    private readonly SmtpConfig _smtpSettings;

    public EmailConsumerService(IConfiguration configuration)
    {
        _configuration = configuration;
        _rabbitMqSettings = _configuration.GetSection("RabbitMqSettings").Get<RabbitMqConfig>();
        _smtpSettings = _configuration.GetSection("Smtp").Get<SmtpConfig>();
    }

    public void Start()
    {
        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqSettings.Host,
            Port = _rabbitMqSettings.Port,
            UserName = _rabbitMqSettings.Username,
            Password = _rabbitMqSettings.Password
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: _rabbitMqSettings.QueueName,
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

        channel.BasicConsume(queue: _rabbitMqSettings.QueueName, autoAck: true, consumer: consumer);
        Console.ReadLine();
    }

    private async Task SendEmailAsync(EmailMessage message)
    {
        var mail = new MailMessage
        {
            From = new MailAddress(_smtpSettings.Username),
            Subject = message.Subject,
            Body = message.Body
        };
        mail.To.Add(message.To);

        using var smtp = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
        {
            Credentials = new System.Net.NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
            EnableSsl = _smtpSettings.EnableSsl
        };

        await smtp.SendMailAsync(mail);
    }
}

