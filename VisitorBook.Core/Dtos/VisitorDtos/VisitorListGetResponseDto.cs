
namespace VisitorBook.Core.Dtos.VisitorDtos
{
    public class VisitorListGetResponseDto
    {
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<VisitorGetResponseDto> Data { get; set; }
    }
}
