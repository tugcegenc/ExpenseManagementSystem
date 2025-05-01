using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Expense.Domain.Entities;

namespace Expense.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(x => x.Id).UseIdentityColumn();

        builder.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(u => u.LastName).IsRequired().HasMaxLength(50);
        builder.Property(u => u.IdentityNumber).IsRequired().HasMaxLength(11);
        builder.Property(u => u.UserName).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.IBAN).IsRequired().HasMaxLength(26);
        builder.Property(u => u.Role).HasConversion<string>().IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired(true);
        builder.Property(x => x.UpdatedAt).IsRequired(false);
        builder.Property(x => x.CreatedBy).IsRequired(true).HasMaxLength(100);
        builder.Property(x => x.UpdatedBy).IsRequired(false).HasMaxLength(100);
        builder.Property(x => x.IsActive).IsRequired(true).HasDefaultValue(true);

        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.IdentityNumber).IsUnique();
        builder.HasIndex(u => u.IBAN).IsUnique();
    }
}

