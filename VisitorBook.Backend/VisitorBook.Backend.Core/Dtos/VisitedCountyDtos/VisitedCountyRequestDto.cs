namespace VisitorBook.Backend.Core.Dtos.VisitedCountyDtos
{
    public class VisitedCountyRequestDto
    {
        public int VisitorId { get; set; }
        public int CityId { get; set; }
        public int CountyId { get; set; }
        public DateTime VisitDate { get; set; }
    }
}
