namespace VisitorBook.Core.Dtos.CountyDtos
{
    public class CountyGetResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public Guid CityId { get; set; }
    }
}
