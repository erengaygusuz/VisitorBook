using VisitorBook.Backend.Core.Dtos.CountryDtos;

namespace VisitorBook.Backend.Core.Dtos.CityDtos
{
    public class CityResponseDto
    {
        public Guid GId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public CountryResponseDto Country { get; set; }
    }
}
