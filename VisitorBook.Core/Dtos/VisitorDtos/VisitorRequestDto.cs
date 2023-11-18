using VisitorBook.Core.Dtos.VisitorAddressDtos;

namespace VisitorBook.Core.Dtos.VisitorDtos
{
    public class VisitorRequestDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public VisitorAddressRequestDto? VisitorAddress { get; set; }
    }
}
