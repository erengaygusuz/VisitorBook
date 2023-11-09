namespace VisitorBook.Core.Dtos.VisitedCountyDtos
{
    public class VisitedCountyUpdateRequestDto
    {
        public Guid Id { get; set; }
        public Guid VisitorId { get; set; }
        public Guid CountyId { get; set; }
        public DateTime VisitDate { get; set; }
    }
}
