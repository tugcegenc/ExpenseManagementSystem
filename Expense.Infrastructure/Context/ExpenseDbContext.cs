using Expense.Application.Services.Interfaces.Sessions;
using Expense.Domain.Base;
using Expense.Domain.Entities;
using Expense.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Expense.Infrastructure.Context;

public class ExpenseDbContext : DbContext
{
    private readonly IAppSession _appSession;
    public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options, IAppSession appSession) : base(options)
    {
        _appSession = appSession;
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

    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)).ToList();

        var auditLogs = new List<AuditLog>();

        foreach (var entry in entries)
        {
            var entity = (BaseEntity)entry.Entity;
            var properties = entry.Properties.ToList();
            var changedProperties = properties.Where(p => p.IsModified).ToList();
            var changedValues = changedProperties.ToDictionary(p => p.Metadata.Name, p => p.CurrentValue);
            var originalValues = properties.ToDictionary(p => p.Metadata.Name, p => p.OriginalValue);

            var auditLog = new AuditLog
            {
                EntityName = entry.Entity.GetType().Name,
                EntityId = entity.Id.ToString(),
                Action = entry.State.ToString(),
                Timestamp = DateTime.UtcNow,
                UserName = _appSession?.UserName ?? "anonymous",
                Role = _appSession?.Role ?? "unknown",
                ChangedValues = JsonConvert.SerializeObject(changedValues),
                OriginalValues = JsonConvert.SerializeObject(originalValues),
            };

            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
                entity.CreatedBy = _appSession?.UserName ?? "anonymous";
                entity.IsActive = true;
            }
            else if (entry.State == EntityState.Modified)
            {
                entity.UpdatedAt = DateTime.UtcNow;
                entity.UpdatedBy = _appSession?.UserName ?? "anonymous";
            }
            else if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entity.UpdatedAt = DateTime.UtcNow;
                entity.UpdatedBy = _appSession?.UserName ?? "anonymous";
                entity.IsActive = false;
            }
            auditLogs.Add(auditLog);
        }
        if (auditLogs.Any())
            await Set<AuditLog>().AddRangeAsync(auditLogs, cancellationToken);
        
        return await base.SaveChangesAsync(cancellationToken);
    }
}