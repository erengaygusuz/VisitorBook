namespace VisitorBook.Core.Dtos.CityDtos
{
    public class CityRequestDto 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int CountryId { get; set; }
    }
}
