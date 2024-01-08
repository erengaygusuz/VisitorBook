using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using VisitorBook.Core.Dtos.RoleDtos;

namespace VisitorBook.Core.ViewModels
{
    public class PermissionViewModel
    {
        public RoleRequestDto Role { get; set; }

        [ValidateNever]
        public List<RoleViewModel> RoleClaims { get; set; }
    }
}
