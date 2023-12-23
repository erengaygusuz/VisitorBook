using VisitorBook.Backend.Core.Dtos.VisitorAddressDtos;

namespace VisitorBook.Backend.Core.Dtos.VisitorDtos
{
    public class VisitorRequestDto
    {
        public int UserId { get; set; }
        public VisitorAddressRequestDto? VisitorAddress { get; set; }
    }
}
