
namespace VisitorBook.Core.Dtos.CountyDtos
{
    public class CountyListResponseDto
    {
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<CountyGetResponseDto> Data { get; set; }
    }
}
