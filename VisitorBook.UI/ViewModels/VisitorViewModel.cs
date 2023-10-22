using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VisitorBook.Core.Models;

namespace VisitorBook.UI.ViewModels
{
    public class VisitorViewModel
    {
        public Visitor Visitor { get; set; }
        [ValidateNever]
        public VisitorAddress VisitorAddress { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> GenderList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CityList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CountyList { get; set; }
    }
}
