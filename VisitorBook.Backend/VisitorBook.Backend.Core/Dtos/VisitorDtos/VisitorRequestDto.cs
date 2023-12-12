using VisitorBook.Backend.Core.Dtos.UserDtos;
using VisitorBook.Backend.Core.Dtos.VisitorAddressDtos;

namespace VisitorBook.Backend.Core.Dtos.VisitorDtos
{
    public class VisitorRequestDto
    {
        public UserRequestDto UserRequestDto { get; set; }
        public VisitorAddressRequestDto? VisitorAddress { get; set; }
    }
}
