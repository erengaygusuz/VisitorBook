namespace VisitorBook.Core.Dtos.CountyDtos
{
    public class CountyRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int CityId { get; set; }
    }
}
