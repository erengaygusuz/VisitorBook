namespace VisitorBook.Core.Dtos.ProfileDtos
{
    public class UpdateSecurityInfoDto
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordOld { get; set; }
        public string PasswordNew { get; set; }
        public string PasswordNewConfirm { get; set; }
    }
}
