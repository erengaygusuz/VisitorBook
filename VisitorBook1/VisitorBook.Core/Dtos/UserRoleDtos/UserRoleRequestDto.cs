namespace VisitorBook.Core.Dtos.UserRoleDtos
{
    public class UserRoleRequestDto
    {
        public int UserId { get; set; }
        public List<UserRoleInfoDto> UserRoleInfo { get; set; }
    }
}
