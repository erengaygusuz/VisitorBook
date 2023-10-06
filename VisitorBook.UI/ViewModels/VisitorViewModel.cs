using Microsoft.AspNetCore.Mvc.Rendering;
using VisitorBook.Core.Models;

namespace VisitorBook.UI.ViewModels
{
    public class VisitorViewModel
    {
        public Visitor Visitor { get; set; }
        public VisitorAddress VisitorAddress { get; set; }
        public IEnumerable<SelectListItem> GenderList { get; set; }
        public IEnumerable<SelectListItem> CityList { get; set; }
        public IEnumerable<SelectListItem> StateList { get; set; }
    }
}
