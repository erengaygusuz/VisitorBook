namespace VisitorBook.Core.Dtos.VisitedCountyDtos
{
    public class VisitedCountyGetResponseDto
    {
        public Guid Id { get; set; }
        public Guid VisitorId { get; set; }
        public string VisitorName { get; set; }
        public Guid CityId { get; set; }
        public string CityName { get; set; }
        public Guid CountyId { get; set; }
        public string CountyName { get; set; }
        public DateTime VisitDate { get; set; }
    }
}
