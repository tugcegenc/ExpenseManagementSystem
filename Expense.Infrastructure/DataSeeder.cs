using Expense.Common.Helpers;
using Expense.Domain.Entities;
using Expense.Domain.Enums;
using Expense.Infrastructure.Context;

namespace Expense.Infrastructure;

public static class DataSeeder
{
    public static async Task SeedAsync(ExpenseDbContext context)
    {
        if (!context.Users.Any())
        {
            const string adminPassword = "Admin123.";
            const string personnelPassword = "User123.";

            var adminSalt = Guid.NewGuid().ToString();
            var personnelSalt = Guid.NewGuid().ToString();

            var adminHash = PasswordGenerator.CreateSHA256(adminPassword, adminSalt);
            var personnelHash = PasswordGenerator.CreateSHA256(personnelPassword, personnelSalt);

            // Admin kullanıcı
            var adminUser = new User
            {
                Id = 1,
                FirstName = "Admin",
                LastName = "User",
                IdentityNumber = "11111111111",
                UserName = "admin",
                Email = "admin@expense.com",
                IBAN = "TR111111111111111111111111",
                PasswordSalt = adminSalt,
                PasswordHash = adminHash,
                Role = UserRole.Admin,
                CreatedAt = new DateTime(2025, 04, 19, 00, 00, 00, DateTimeKind.Utc),
                CreatedBy = "seed",
                IsActive = true
            };

            // Personel kullanıcı
            var personnelUser = new User
            {
                Id = 2,
                FirstName = "Personnel",
                LastName = "User",
                IdentityNumber = "22222222222",
                UserName = "personnel",
                Email = "user@expense.com",
                IBAN = "TR222222222222222222222222",
                PasswordSalt = personnelSalt,
                PasswordHash = personnelHash,
                Role = UserRole.Personnel,
                CreatedAt = new DateTime(2025, 04, 19, 00, 00, 00, DateTimeKind.Utc),
                CreatedBy = "seed",
                IsActive = true
            };

            await context.Users.AddRangeAsync(adminUser, personnelUser);
            await context.SaveChangesAsync();

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("Default users created:");
            Console.ResetColor();
            Console.WriteLine("Admin     => Email: admin@expense.com   | Password: Admin123.");
            Console.WriteLine("Personnel => Email: user@expense.com    | Password: User123.");
        }
    }
}
