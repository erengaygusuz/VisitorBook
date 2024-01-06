namespace VisitorBook.Core.Dtos.AuditTrailDtos
{
    public class AuditTrailResponseDto
    {
        public string Username { get; set; }
        public string Type { get; set; }
        public string TableName { get; set; }
        public string CreatedDate { get; set; }
        public Dictionary<string, string> OldValues { get; set; }
        public Dictionary<string, string> NewValues { get; set; }
        public List<string> AffectedColumns { get; set; }
        public Dictionary<string, string> PrimaryKey { get; set; }
    }
}
