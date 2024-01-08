using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Dtos.AuthDtos;
using VisitorBook.Core.Entities;
using VisitorBook.UI.Languages;

namespace VisitorBook.UI.Validators
{
    public class ForgotPasswordRequestDtoValidator : AbstractValidator<ForgotPasswordRequestDto>
    {
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer<Language> _localization;

        public ForgotPasswordRequestDtoValidator(UserManager<User> userManager, IStringLocalizer<Language> localization)
        {
            _localization = localization;
            _userManager = userManager;

            RuleFor(x => x.Email)
                .NotNull().WithMessage(_localization["Validators.ForgotPassword.Message1.Text"].Value)
                .EmailAddress().WithMessage(_localization["Validators.ForgotPassword.Message2.Text"].Value)
                .Must(NotExistEmail).WithMessage(_localization["Validators.ForgotPassword.Message3.Text"].Value);
        }

        private bool NotExistEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            var user = _userManager.FindByEmailAsync(email).GetAwaiter().GetResult();

            if (user == null)
            {
                return false;
            }

            return true;
        }
    }
}
