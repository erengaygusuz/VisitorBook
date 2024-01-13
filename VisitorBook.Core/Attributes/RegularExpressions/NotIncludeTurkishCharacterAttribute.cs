using System.ComponentModel.DataAnnotations;

namespace VisitorBook.Core.Attributes.RegularExpressions
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class NotIncludeTurkishCharacterAttribute : RegularExpressionAttribute
    {
        public NotIncludeTurkishCharacterAttribute() : base("^((?![ğĞçÇşŞüÜöÖıİ]).)*$")
        {
            
        }
    }
}
