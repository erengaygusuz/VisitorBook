namespace VisitorBook.Core.Dtos.RegisterApplicationDto
{
    public class RegisterApplicationCreateRequestDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string Explanation { get; set; }

        public string Status { get; set; }
    }
}
