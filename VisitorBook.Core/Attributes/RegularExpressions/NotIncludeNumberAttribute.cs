using System.ComponentModel.DataAnnotations;

namespace VisitorBook.Core.Attributes.RegularExpressions
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class NotIncludeNumberAttribute : RegularExpressionAttribute
    {
        public NotIncludeNumberAttribute() : base("^((?![0-9]).)*$")
        {
            
        }
    }
}
