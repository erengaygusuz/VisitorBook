using VisitorBook.Core.Dtos.RegionDtos;

namespace VisitorBook.Core.Dtos.SubRegionDtos
{
    public class SubRegionResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public RegionResponseDto Region { get; set; }
    }
}
