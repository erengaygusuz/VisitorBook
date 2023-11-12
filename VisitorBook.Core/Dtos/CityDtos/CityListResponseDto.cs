
namespace VisitorBook.Core.Dtos.CityDtos
{
    public class CityListResponseDto
    {
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<CityGetResponseDto> Data { get; set; }
    }
}
