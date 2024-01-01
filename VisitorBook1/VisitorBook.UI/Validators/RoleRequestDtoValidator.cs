using FluentValidation;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Dtos.RoleDtos;
using VisitorBook.UI.Languages;

namespace VisitorBook.UI.Validators
{
    public class RoleRequestDtoValidator : AbstractValidator<RoleRequestDto>
    {
        private readonly IStringLocalizer<Language> _localization;

        public RoleRequestDtoValidator(IStringLocalizer<Language> localization)
        {
            _localization = localization;

            RuleFor(x => x.Name)
                .NotNull().WithMessage(_localization["Validators.Role.Message1.Text"].Value)
                .Matches("^((?![0-9]).)*$").WithMessage(_localization["Validators.Role.Message2.Text"].Value)
                .MinimumLength(3).WithMessage(_localization["Validators.Role.Message3.Text"].Value)
                .MaximumLength(100).WithMessage(_localization["Validators.Role.Message4.Text"].Value);
        }
    }
}
