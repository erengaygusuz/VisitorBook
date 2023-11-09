namespace VisitorBook.Core.Dtos.VisitedCountyDtos
{
    public class VisitedCountyAddRequestDto
    {
        public Guid VisitorId { get; set; }
        public Guid CountyId { get; set; }
        public DateTime VisitDate { get; set; }
    }
}
