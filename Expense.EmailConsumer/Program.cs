using Microsoft.Extensions.Configuration;
using Expense.EmailConsumer.Services;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile("appsettings.Local.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

IEmailConsumerService consumer = new EmailConsumerService(configuration);
consumer.Start();