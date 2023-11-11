using VisitorBook.Core.Dtos.VisitorAddressDtos;
using VisitorBook.Core.Enums;

namespace VisitorBook.Core.Dtos.VisitorDtos
{
    public class VisitorGetResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public Guid CityId { get; set; }
        public Guid CountyId { get; set; }
        public Guid? VisitorAddressId { get; set; }
    }
}
