using VisitorBook.Core.Dtos.VisitorAddressDtos;

namespace VisitorBook.Core.Dtos.ProfileDtos
{
    public class UpdateGeneralInfoDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }

        public UserAddressRequestDto? UserAddress { get; set; }
    }
}
