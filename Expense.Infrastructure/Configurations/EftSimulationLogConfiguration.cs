using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Expense.Domain.Entities;

namespace Expense.Infrastructure.Configurations;

public class EftSimulationLogConfiguration : IEntityTypeConfiguration<EftSimulationLog>
{
    public void Configure(EntityTypeBuilder<EftSimulationLog> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).UseIdentityColumn();

        builder.Property(e => e.Amount).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(e => e.SimulatedAt).IsRequired();
        builder.Property(e => e.IsSuccessful).IsRequired().HasDefaultValue(false);

        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired(false);
        builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(100);
        builder.Property(x => x.UpdatedBy).IsRequired(false).HasMaxLength(100);
        builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);
        
        builder.HasOne(e => e.ExpenseClaim)
               .WithMany(r => r.EftSimulationLogs)
               .HasForeignKey(e => e.ExpenseClaimId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

