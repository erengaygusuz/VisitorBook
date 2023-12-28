using VisitorBook.Backend.Core.Dtos.UserDtos;
using VisitorBook.Backend.Core.Dtos.VisitorAddressDtos;

namespace VisitorBook.Backend.Core.Dtos.VisitorDtos
{
    public class VisitorResponseDto
    {
        public UserResponseDto User { get; set; }
        public VisitorAddressResponseDto? VisitorAddress { get; set; }
    }
}
