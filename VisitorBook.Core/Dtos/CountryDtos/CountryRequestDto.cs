namespace VisitorBook.Core.Dtos.CountryDtos
{
    public class CountryRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int SubRegionId { get; set; }
    }
}
