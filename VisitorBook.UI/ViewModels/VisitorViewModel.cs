using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VisitorBook.UI.Models.Inputs;

namespace VisitorBook.UI.ViewModels
{
    public class VisitorViewModel
    {
        public VisitorInput Visitor { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> GenderList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CityList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CountyList { get; set; }
    }
}
