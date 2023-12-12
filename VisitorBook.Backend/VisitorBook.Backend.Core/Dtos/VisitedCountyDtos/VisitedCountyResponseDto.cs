using VisitorBook.Backend.Core.Dtos.CountyDtos;
using VisitorBook.Backend.Core.Dtos.VisitorDtos;

namespace VisitorBook.Backend.Core.Dtos.VisitedCountyDtos
{
    public class VisitedCountyResponseDto
    {
        public Guid GId { get; set; }
        public VisitorResponseDto Visitor { get; set; }
        public CountyResponseDto County { get; set; }
        public DateTime VisitDate { get; set; }
    }
}
