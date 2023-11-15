using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VisitorBook.Core.Dtos.VisitedCountyDtos;

namespace VisitorBook.UI.ViewModels
{
    public class VisitedCountyAddViewModel
    {
        public VisitedCountyRequestDto VisitedCountyRequestDto { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> VisitorList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CountyList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CityList { get; set; }
    }
}
