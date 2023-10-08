using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VisitorBook.Core.Models;

namespace VisitorBook.UI.ViewModels
{
    public class VisitedStateViewModel
    {
        public VisitedState VisitedState { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> VisitorList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> StateList { get; set; }
    }
}
