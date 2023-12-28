using VisitorBook.Core.Dtos.CountryDtos;

namespace VisitorBook.Core.Dtos.CityDtos
{
    public class CityResponseDto 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public CountryResponseDto Country { get; set; }
    }
}
