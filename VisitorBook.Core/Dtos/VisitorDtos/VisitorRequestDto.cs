using VisitorBook.Core.Enums;

namespace VisitorBook.Core.Dtos.VisitorDtos
{
    public class VisitorRequestDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public Guid CityId { get; set; }
        public Guid CountyId { get; set; }
    }
}
