using VisitorBook.Core.Dtos.SubRegionDtos;

namespace VisitorBook.Core.Dtos.CountryDtos
{
    public class CountryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public SubRegionResponseDto SubRegion { get; set; }
    }
}
