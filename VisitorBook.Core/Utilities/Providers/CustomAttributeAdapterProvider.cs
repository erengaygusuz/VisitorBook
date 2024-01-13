using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using VisitorBook.Core.Attributes.RegularExpressions;
using VisitorBook.Core.Utilities.Adapters;

namespace VisitorBook.Core.Utilities.Providers
{
    public class CustomAttributeAdapterProvider : IValidationAttributeAdapterProvider
    {
        private readonly IValidationAttributeAdapterProvider _baseProvider = new ValidationAttributeAdapterProvider();

        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            if (attribute is NotIncludeNumberAttribute)
            {
                return new NotIncludeNumberAttributeAdapter(attribute as NotIncludeNumberAttribute, stringLocalizer);
            }
            else if (attribute is NotIncludeTurkishCharacterAttribute)
            {
                return new NotIncludeTurkishCharacterAttributeAdapter(attribute as NotIncludeTurkishCharacterAttribute, stringLocalizer);
            }
            else
            {
                return _baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
            }
        }
    }
}
