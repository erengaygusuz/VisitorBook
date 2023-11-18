using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VisitorBook.UI.Models;

namespace VisitorBook.UI.ViewModels
{
    public class VisitedCountyViewModel
    {
        public VisitedCounty VisitedCounty { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> VisitorList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CountyList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CityList { get; set; }
    }
}
