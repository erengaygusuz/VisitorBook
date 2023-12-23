using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VisitorBook.Frontend.UI.Models.Inputs;

namespace VisitorBook.Frontend.UI.ViewModels
{
    public class SubRegionViewModel
    {
        public SubRegionInput SubRegion { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> RegionList { get; set; }
    }
}
