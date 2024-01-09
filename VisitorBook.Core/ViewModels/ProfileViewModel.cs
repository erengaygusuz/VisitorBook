using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VisitorBook.Core.Dtos.ProfileDtos;
using VisitorBook.Core.Dtos.VisitorAddressDtos;

namespace VisitorBook.Core.ViewModels
{
    public class ProfileViewModel
    { 
        public UpdateSecurityInfoDto UserSecurityInfo { get; set; }

        public UpdateGeneralInfoDto UserGeneralInfo { get; set; }

        public UserAddressRequestDto? UserAddress { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> GenderList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CityList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CountyList { get; set; }
    }
}
