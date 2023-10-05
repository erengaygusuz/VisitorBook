using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VisitorBook.Core.Models;

namespace VisitorBook.UI.ViewModels
{
    public class StateViewModel
    {
        public State State { get; set; }
        public IEnumerable<SelectListItem> CityList { get; set; }
    }
}
