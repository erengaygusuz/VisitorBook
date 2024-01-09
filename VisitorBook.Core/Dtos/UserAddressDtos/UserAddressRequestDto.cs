namespace VisitorBook.Core.Dtos.VisitorAddressDtos
{
    public class UserAddressRequestDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CityId { get; set; }
        public int CountyId { get; set; }
    }
}