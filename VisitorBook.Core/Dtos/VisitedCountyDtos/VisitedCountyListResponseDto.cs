
namespace VisitorBook.Core.Dtos.VisitedCountyDtos
{
    public class VisitedCountyListResponseDto
    {
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<VisitedCountyGetResponseDto> Data { get; set; }
    }
}
