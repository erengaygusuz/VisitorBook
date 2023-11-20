namespace VisitorBook.Backend.Core.Dtos.VisitedCountyDtos
{
    public class VisitedCountyRequestDto
    {
        public Guid VisitorId { get; set; }
        public Guid CityId { get; set; }
        public Guid CountyId { get; set; }
        public DateTime VisitDate { get; set; }
    }
}
