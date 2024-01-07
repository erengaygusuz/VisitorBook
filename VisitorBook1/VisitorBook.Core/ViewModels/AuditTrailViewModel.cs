using VisitorBook.Core.Dtos.AuditTrailDtos;

namespace VisitorBook.Core.ViewModels
{
    public class AuditTrailViewModel
    {
        public string Username { get; set; }

        public string Type { get; set; }

        public string TableName { get; set; }

        public string CreatedDate { get; set; }

        public Dictionary<string, string> PrimaryKey { get; set; }

        public List<AuditTrailAffectedColumnResponseDto> AffectedColumns { get; set; }
    }
}
