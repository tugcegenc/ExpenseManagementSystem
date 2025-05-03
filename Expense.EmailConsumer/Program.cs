using Expense.EmailConsumer.Services;

IEmailConsumerService consumer = new EmailConsumerService();
consumer.Start();