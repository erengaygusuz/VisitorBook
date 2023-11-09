using VisitorBook.Core.Enums;

namespace VisitorBook.Core.Dtos.VisitorDtos
{
    public class VisitorAddRequestDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public Guid? VisitorAddressId { get; set; }
    }
}
