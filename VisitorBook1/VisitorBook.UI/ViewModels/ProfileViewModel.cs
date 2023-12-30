using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VisitorBook.Core.Dtos.ProfileDtos;

namespace VisitorBook.UI.ViewModels
{
    public class ProfileViewModel
    { 
        public UpdateSecurityInfoDto UserSecurityInfo { get; set; }

        public UpdateGeneralInfoDto UserGeneralInfo { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> GenderList { get; set; }
    }
}
