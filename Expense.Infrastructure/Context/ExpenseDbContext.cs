using Expense.Domain.Entities;
using Expense.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Expense.Infrastructure.Context;

public class ExpenseDbContext : DbContext
{
    public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options) : base(options)
    {
    }

    public DbSet<ExpenseClaim> ExpenseClaims { get; set; }
    public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<EftSimulationLog> EftSimulationLogs { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExpenseDbContext).Assembly);
        base.OnModelCreating(modelBuilder);

        // Seed ExpenseCtegory
        modelBuilder.Entity<ExpenseCategory>().HasData(
            new ExpenseCategory
            {
                Id = 1,
                Name = "Yemek",
                Description = "Yemek giderleri",
                CreatedAt = new DateTime(2025, 04, 19, 00, 00, 00, DateTimeKind.Utc),
                CreatedBy = "seed",
                IsActive = true
            },
            new ExpenseCategory
            {
                Id = 2,
                Name = "Ulaşım",
                Description = "Ulaşım giderleri",
                CreatedAt = new DateTime(2025, 04, 19, 00, 00, 00, DateTimeKind.Utc),
                CreatedBy = "seed",
                IsActive = true
            },
            new ExpenseCategory
            {
                Id = 3,
                Name = "Konaklama",
                Description = "Otel, pansiyon vb.",
                CreatedAt = new DateTime(2025, 04, 19, 00, 00, 00, DateTimeKind.Utc),
                CreatedBy = "seed",
                IsActive = true
            }
        );

        // Seed User
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                FirstName = "Admin",
                LastName = "User",
                IdentityNumber = "11111111111",
                UserName = "Admin",
                Email = "admin@expense.com",
                IBAN = "TR000000000000000000000001",
                PasswordSalt = "seededSalt",
                /////password = "admin123"/////
                PasswordHash = "EEB17C526DC68D7531DFE22A0A0031EBDD0C7213C4F83D6937589C0028BDCE06", 
                Role = UserRole.Admin,
                CreatedAt = new DateTime(2025, 04, 19, 00, 00, 00, DateTimeKind.Utc),
                CreatedBy = "seed",
                IsActive = true
            });


    }
}