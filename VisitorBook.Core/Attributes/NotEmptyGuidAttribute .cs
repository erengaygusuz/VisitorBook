using System.ComponentModel.DataAnnotations;

namespace VisitorBook.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class NotEmptyGuidAttribute : ValidationAttribute
    {
        public const string ErrorMessage = "The {0} field must not be empty";
        public NotEmptyGuidAttribute() : base(ErrorMessage) { }
        public override bool IsValid(object value)
        {
            if (value is null)
            {
                return true; // Allows to return a null value
            }
                
            switch (value)
            {
                case Guid guid:
                    return guid != Guid.Empty; //Checks whether the GUID is empty or not and returns false if GUID is empty
                default:
                    return true;
            }
        }
    }
}
