using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VisitorBook.Core.Extensions
{
    public static class ModelStateExtensions
    {
        public static IEnumerable<string> GetValidationErrors(this ModelStateDictionary modelState)
        {
            return modelState.Values.SelectMany(modelStateEntry => modelStateEntry.Errors).Select(error => error.ErrorMessage);
        }
    }
}
