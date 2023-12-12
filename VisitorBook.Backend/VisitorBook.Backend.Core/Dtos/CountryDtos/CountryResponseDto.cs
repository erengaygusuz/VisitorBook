using VisitorBook.Backend.Core.Dtos.SubRegionDtos;

namespace VisitorBook.Backend.Core.Dtos.CountryDtos
{
    public class CountryResponseDto
    {
        public Guid GId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public SubRegionResponseDto SubRegion { get; set; }
    }
}
