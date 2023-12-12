using VisitorBook.Backend.Core.Dtos.RegionDtos;

namespace VisitorBook.Backend.Core.Dtos.SubRegionDtos
{
    public class SubRegionResponseDto
    {
        public Guid GId { get; set; }
        public string Name { get; set; }
        public RegionResponseDto Region { get; set; }
    }
}
