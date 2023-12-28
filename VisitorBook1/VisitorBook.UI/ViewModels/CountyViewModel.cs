using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VisitorBook.Core.Dtos.CountyDtos;

namespace VisitorBook.UI.ViewModels
{
    public class CountyViewModel
    {
        public CountyRequestDto County { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CityList { get; set; }
    }
}
