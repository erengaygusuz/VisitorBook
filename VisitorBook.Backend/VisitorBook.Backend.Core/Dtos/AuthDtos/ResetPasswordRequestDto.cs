namespace VisitorBook.Backend.Core.Dtos.AuthDtos
{
    public class ResetPasswordRequestDto
    {
        public string Password { get; set; }

        public string PasswordConfirm { get; set; }
    }
}
