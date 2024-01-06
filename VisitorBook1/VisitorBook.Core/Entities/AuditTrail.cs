namespace VisitorBook.Core.Entities
{
    public class AuditTrail
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string? Type { get; set; }

        public string? TableName { get; set; }

        public DateTime TimeStamp { get; set; }

        public string? OldValues { get; set; }

        public string? NewValues { get; set; }

        public string? AffectedColumns { get; set; }

        public string? PrimaryKey { get; set; }
    }
}
