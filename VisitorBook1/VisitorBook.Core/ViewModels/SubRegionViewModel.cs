using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VisitorBook.Core.Dtos.SubRegionDtos;

namespace VisitorBook.Core.ViewModels
{
    public class SubRegionViewModel
    {
        public SubRegionRequestDto SubRegion { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> RegionList { get; set; }
    }
}
