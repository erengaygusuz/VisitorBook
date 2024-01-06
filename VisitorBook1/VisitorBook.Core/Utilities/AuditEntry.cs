using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using VisitorBook.Core.Entities;
using VisitorBook.Core.Enums;

namespace VisitorBook.Core.Utilities
{
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        public EntityEntry Entry { get; }

        public string Username { get; set; }

        public string TableName { get; set; }

        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();

        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();

        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();

        public AuditType AuditType { get; set; }

        public List<string> ChangedColumns { get; } = new List<string>();

        public AuditTrail ToAudit()
        {
            var audit = new AuditTrail();

            audit.Username = Username;
            audit.Type = AuditType.ToString();
            audit.TableName = TableName;
            audit.TimeStamp = DateTime.Now;
            audit.PrimaryKey = JsonConvert.SerializeObject(KeyValues);
            audit.OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues);
            audit.NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues);
            audit.AffectedColumns = ChangedColumns.Count == 0 ? null : JsonConvert.SerializeObject(ChangedColumns);

            return audit;
        }
    }
}
