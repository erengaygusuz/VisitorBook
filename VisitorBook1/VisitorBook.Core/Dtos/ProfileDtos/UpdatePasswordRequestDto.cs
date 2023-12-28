namespace VisitorBook.Core.Dtos.ProfileDtos
{
    public class UpdatePasswordRequestDto
    {
        public string PasswordOld { get; set; }
        public string PasswordNew { get; set; }
        public string PasswordNewConfirm { get; set; }
    }
}
