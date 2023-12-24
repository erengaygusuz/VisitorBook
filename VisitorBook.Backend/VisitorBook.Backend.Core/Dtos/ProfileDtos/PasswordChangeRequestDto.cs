namespace VisitorBook.Backend.Core.Dtos.ProfileDtos
{
    public class PasswordChangeRequestDto
    {
        public string PasswordOld { get; set; }

        public string PasswordNew { get; set; }

        public string PasswordNewConfirm { get; set; }
    }
}
