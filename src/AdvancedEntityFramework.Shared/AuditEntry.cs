using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedEntityFramework.Shared.Entities.Audits;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace AdvancedEntityFramework.Shared
{
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        public EntityEntry Entry { get; }
        public bool HasTemporaryProperties => TemporaryProperties.Any();
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public string TableName { get; set; }
        public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();

        public AuditEntity ToAuditEntity(Guid transactionId)
        {
            return new AuditEntity
            {
                CreatedAt = DateTime.UtcNow,
                KeyValues = JsonConvert.SerializeObject(KeyValues),
                NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues),
                OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues),
                TableName = TableName,
                TransactionId = transactionId
            };
        }
    }
}