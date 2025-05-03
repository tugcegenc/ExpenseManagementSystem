using Expense.Common.RabbitMQ;

public static class EmailContentBuilder
{
    public static EmailMessage BuildUserCreatedMessage(UserCreatedMessage message)
    {
        return new EmailMessage
        {
            To = message.Email,
            Subject = "Welcome to Expense Management System",
            Body = $@"
        Hello {message.FullName},

        Your account has been successfully created in the Expense Management System.

        👤 Username: {message.UserName}  
        🔐 Password: {message.UserPassword}

        You can now log in to the system using these credentials.

        Best regards,  
        Expense Management System Team"
        };

    }
}