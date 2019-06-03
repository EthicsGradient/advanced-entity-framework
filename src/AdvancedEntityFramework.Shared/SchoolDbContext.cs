using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AdvancedEntityFramework.Shared.Entities;
using AdvancedEntityFramework.Shared.Entities.Audits;
using AdvancedEntityFramework.Shared.Entities.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AdvancedEntityFramework.Shared
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<AuditEntity> Audits { get; set; }
        public DbSet<StudentEntity> Students { get; set; }

        private List<AuditEntry> OnBeforeSaveChanges(Guid transactionId)
        {
            ChangeTracker.DetectChanges();

            var auditEntries = new List<AuditEntry>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is AuditEntity || entry.State == EntityState.Detached ||
                    entry.State == EntityState.Unchanged)
                {
                    continue;
                }

                var auditEntry = new AuditEntry(entry)
                {
                    TableName = entry.Metadata.GetAnnotation("Relational:TableName").Value.ToString()
                };

                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    var propertyName = property.Metadata.Name;

                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }

                            break;
                    }
                }
            }

            foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties))
            {
                Audits.Add(auditEntry.ToAuditEntity(transactionId));
            }

            return auditEntries.Where(_ => _.HasTemporaryProperties).ToList();
        }

        private Task OnAfterSaveChanges(Guid transactionId, IReadOnlyCollection<AuditEntry> auditEntries)
        {
            if (auditEntries == null || !auditEntries.Any())
            {
                return Task.CompletedTask;
            }

            foreach (var auditEntry in auditEntries)
            {
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    else
                    {
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }

                Audits.Add(auditEntry.ToAuditEntity(transactionId));
            }

            return SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assemblyWithConfigurations = GetType().Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assemblyWithConfigurations);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            throw new NotImplementedException("Use SaveChangesAsync()");
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken)
        {
            foreach (var entity in ChangeTracker.Entries()
                .Where(e => e.Entity is IEntity && e.State == EntityState.Added)
                .Select(e => e.Entity)
                .Cast<IEntity>())
            {
                entity.CreatedAt = DateTime.UtcNow;
                entity.UpdatedAt = DateTime.UtcNow;
            }

            foreach (var entity in ChangeTracker.Entries()
                .Where(e => e.Entity is IEntity && e.State == EntityState.Modified)
                .Select(e => e.Entity)
                .Cast<IEntity>())
            {
                entity.UpdatedAt = DateTime.UtcNow;
            }

            var transactionId = Guid.NewGuid();
            var auditEntries = OnBeforeSaveChanges(transactionId);
            
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            await OnAfterSaveChanges(transactionId, auditEntries);

            return result;
        }
    }
}
