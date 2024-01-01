using FluentValidation;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Dtos.AuthDtos;
using VisitorBook.UI.Languages;

namespace VisitorBook.UI.Validators
{
    public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
    {
        private readonly IStringLocalizer<Language> _localization;

        public LoginRequestDtoValidator(IStringLocalizer<Language> localization)
        {
            _localization = localization;

            RuleFor(x => x.Email).NotNull().WithMessage(_localization["Validators.Login.Message1.Text"].Value);
            RuleFor(x => x.Password).NotNull().WithMessage(_localization["Validators.Login.Message2.Text"].Value);
        }
    }
}
