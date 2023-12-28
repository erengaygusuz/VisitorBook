using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VisitorBook.Core.Extensions
{
    public static class ModelStateExtensions
    {
        public static IEnumerable<string> GetValidationErrors(this ModelStateDictionary modelState)
        {
            return modelState.Values.SelectMany(modelStateEntry => modelStateEntry.Errors).Select(error => error.ErrorMessage);
        }

        public static void AddModelErrorList(this ModelStateDictionary modelState, List<string> errors)
        {
            errors.ForEach(x =>
            {
                modelState.AddModelError(string.Empty, x);
            });
        }

        public static void AddModelErrorList(this ModelStateDictionary modelState, IEnumerable<IdentityError> errors)
        {
            errors.ToList().ForEach(x =>
            {
                modelState.AddModelError(string.Empty, x.Description);
            });
        }
    }
}
