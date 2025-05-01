using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Expense.Domain.Entities;

namespace Expense.Infrastructure.Configurations;

public class ExpenseClaimConfiguration : IEntityTypeConfiguration<ExpenseClaim>
{
       public void Configure(EntityTypeBuilder<ExpenseClaim> builder)
       {
              builder.HasKey(e => e.Id);
              builder.Property(e => e.Id).UseIdentityColumn();

              builder.Property(e => e.Amount).IsRequired().HasColumnType("decimal(18,2)");
              builder.Property(e => e.RequestDate).IsRequired();
              builder.Property(e => e.Description).IsRequired().HasMaxLength(500);
              builder.Property(e => e.ApprovedOrRejectedDate).IsRequired(false);
              builder.Property(e => e.RejectReason).HasMaxLength(250);
              builder.Property(e => e.Location).IsRequired().HasMaxLength(150);

              builder.Property(e => e.PaymentMethod).IsRequired().HasConversion<string>();  
              builder.Property(e => e.Status).IsRequired().HasConversion<string>();
              builder.Property(e => e.PaymentStatus).IsRequired().HasConversion<string>();
              
              builder.Property(x => x.CreatedAt).IsRequired();
              builder.Property(x => x.UpdatedAt).IsRequired(false);
              builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(100);
              builder.Property(x => x.UpdatedBy).IsRequired(false).HasMaxLength(100);
              builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);



              builder.HasOne(e => e.User)
                     .WithMany(u => u.ExpenseClaims)
                     .HasForeignKey(e => e.UserId)
                     .OnDelete(DeleteBehavior.Restrict);

              builder.HasOne(e => e.ExpenseCategory)
                     .WithMany(c => c.ExpenseClaims)
                     .HasForeignKey(e => e.ExpenseCategoryId)
                     .OnDelete(DeleteBehavior.Restrict);

              builder.HasMany(e => e.EftSimulationLogs)
                     .WithOne(log => log.ExpenseClaim)
                     .HasForeignKey(log => log.ExpenseClaimId)
                     .OnDelete(DeleteBehavior.Cascade);
       }
}

