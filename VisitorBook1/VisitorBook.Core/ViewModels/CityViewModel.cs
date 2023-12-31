using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VisitorBook.Core.Dtos.CityDtos;

namespace VisitorBook.Core.ViewModels
{
    public class CityViewModel
    {
        public CityRequestDto City { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CountryList { get; set; }
    }
}
