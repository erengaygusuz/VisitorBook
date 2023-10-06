using Microsoft.AspNetCore.Mvc.Rendering;
using VisitorBook.Core.Models;

namespace VisitorBook.UI.ViewModels
{
    public class VisitedStateViewModel
    {
        public VisitedState VisitedState { get; set; }
        public IEnumerable<SelectListItem> VisitorList { get; set; }
        public IEnumerable<SelectListItem> StateList { get; set; }
    }
}
