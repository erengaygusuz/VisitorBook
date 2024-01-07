namespace VisitorBook.Core.Dtos.RegisterApplicationDto
{
    public class RegisterApplicationRequestDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Explanation { get; set; }

        public string Status { get; set; }
    }
}
