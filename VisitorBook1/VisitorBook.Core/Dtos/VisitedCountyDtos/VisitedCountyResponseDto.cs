using VisitorBook.Core.Dtos.CountyDtos;
using VisitorBook.Core.Dtos.UserDtos;

namespace VisitorBook.Core.Dtos.VisitedCountyDtos
{
    public class VisitedCountyResponseDto
    {
        public int Id { get; set; }
        public UserResponseDto User { get; set; }
        public CountyResponseDto County { get; set; }
        public DateTime VisitDate { get; set; }
    }
}
