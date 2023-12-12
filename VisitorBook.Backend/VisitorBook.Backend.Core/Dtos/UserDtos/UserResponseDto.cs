namespace VisitorBook.Backend.Core.Dtos.UserDtos
{
    public class UserResponseDto
    {
        public Guid GId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
    }
}
