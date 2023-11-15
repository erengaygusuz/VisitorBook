namespace VisitorBook.Core.Dtos.CountyDtos
{
    public class CountyRequestDto
    {
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public Guid CityId { get; set; }
    }
}
