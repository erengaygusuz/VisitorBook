namespace VisitorBook.Core.Dtos.AuditTrailDtos
{
    public class AuditTrailAffectedColumnResponseDto
    {
        public string ColumnName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}
