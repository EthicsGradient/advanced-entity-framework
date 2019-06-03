using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdvancedEntityFramework.Shared.Entities.Audits
{
    [Table("Audit")]
    public class AuditEntity
    {
        public Guid AuditId { get; set; }
        public string KeyValues { get; set; }
        public string NewValues { get; set; }
        public string OldValues { get; set; }
        public string TableName { get; set; }
        public Guid TransactionId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}