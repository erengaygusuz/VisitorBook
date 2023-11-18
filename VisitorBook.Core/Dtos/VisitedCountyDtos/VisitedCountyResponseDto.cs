using VisitorBook.Core.Dtos.CountyDtos;
using VisitorBook.Core.Dtos.VisitorDtos;

namespace VisitorBook.Core.Dtos.VisitedCountyDtos
{
    public class VisitedCountyResponseDto
    {
        public Guid Id { get; set; }
        public VisitorResponseDto Visitor { get; set; }
        public CountyResponseDto County { get; set; }
        public DateTime VisitDate { get; set; }
    }
}
