using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdvancedEntityFramework.Shared.Entities.Audits
{
    public class AuditEntityConfiguration : IEntityTypeConfiguration<AuditEntity>
    {
        public void Configure(EntityTypeBuilder<AuditEntity> builder)
        {
            builder.HasKey(x => x.AuditId);

            builder.HasIndex(x => x.TableName);
            builder.HasIndex(x => x.TransactionId);
            builder.HasIndex(x => x.CreatedAt);
        }
    }
}