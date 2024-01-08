using VisitorBook.Core.Dtos.CityDtos;

namespace VisitorBook.Core.Dtos.CountyDtos
{
    public class CountyResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public CityResponseDto City { get; set; }
    }
}
