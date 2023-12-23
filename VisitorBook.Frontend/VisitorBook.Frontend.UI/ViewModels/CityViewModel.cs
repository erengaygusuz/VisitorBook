using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VisitorBook.Frontend.UI.Models.Inputs;

namespace VisitorBook.Frontend.UI.ViewModels
{
    public class CityViewModel
    {
        public CityInput City { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CountryList { get; set; }
    }
}
