using VisitorBook.Backend.Core.Dtos.VisitorAddressDtos;

namespace VisitorBook.Backend.Core.Dtos.VisitorDtos
{
    public class VisitorResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public VisitorAddressResponseDto? VisitorAddress { get; set; }
    }
}
