using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VisitorBook.Core.Dtos.RegisterApplicationDto;

namespace VisitorBook.Core.ViewModels
{
    public class RegisterApplicationViewModel
    {
        public RegisterApplicationRequestDto RegisterApplication { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> RegisterApplicationStatusList { get; set; }
    }
}
