using Expense.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expense.Infrastructure.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();

        builder.Property(x => x.EntityName).IsRequired(false).HasMaxLength(100);
        builder.Property(x => x.EntityId).IsRequired(false).HasMaxLength(100);
        builder.Property(x => x.Action).IsRequired(false).HasMaxLength(100);
        builder.Property(x => x.Timestamp).IsRequired(false);
        builder.Property(x => x.UserName).IsRequired(false).HasMaxLength(100);

        builder.Property(x => x.ChangedValues).IsRequired(false);
        builder.Property(x => x.OriginalValues).IsRequired(false);
    }
}