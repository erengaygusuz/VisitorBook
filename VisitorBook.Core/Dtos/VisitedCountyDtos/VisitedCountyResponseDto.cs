namespace VisitorBook.Core.Dtos.VisitedCountyDtos
{
    public class VisitedCountyResponseDto
    {
        public Guid Id { get; set; }
        public string VisitorNameSurname { get; set; }
        public string CityName { get; set; }
        public string CountyName { get; set; }
        public DateTime VisitDate { get; set; }
    }
}
