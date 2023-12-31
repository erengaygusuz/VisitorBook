using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VisitorBook.Core.Dtos.CountryDtos;

namespace VisitorBook.Core.ViewModels
{
    public class CountryViewModel
    {
        public CountryRequestDto Country { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> SubRegionList { get; set; }
    }
}
