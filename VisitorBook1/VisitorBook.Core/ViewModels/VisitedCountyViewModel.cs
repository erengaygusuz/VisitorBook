using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VisitorBook.Core.Dtos.VisitedCountyDtos;

namespace VisitorBook.Core.ViewModels
{
    public class VisitedCountyViewModel
    {
        public VisitedCountyRequestDto VisitedCounty { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> VisitorList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CountyList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CityList { get; set; }
    }
}
