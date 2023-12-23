using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VisitorBook.Frontend.UI.Models.Inputs;

namespace VisitorBook.Frontend.UI.ViewModels
{
    public class CountryViewModel
    {
        public CountryInput Country { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> SubRegionList { get; set; }
    }
}
