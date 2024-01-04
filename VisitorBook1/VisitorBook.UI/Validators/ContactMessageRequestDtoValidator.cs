using FluentValidation;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Dtos.ContactMessageDtos;
using VisitorBook.UI.Languages;

namespace VisitorBook.UI.Validators
{
    public class ContactMessageRequestDtoValidator : AbstractValidator<ContactMessageRequestDto>
    {
        private readonly IStringLocalizer<Language> _localization;

        public ContactMessageRequestDtoValidator(IStringLocalizer<Language> localization)
        {
            _localization = localization; 

            RuleFor(x => x.NameSurname)
                .NotNull().WithMessage(_localization["Validators.ContactMessage.Message1.Text"].Value)
                .Matches("^((?![0-9]).)*$").WithMessage(_localization["Validators.ContactMessage.Message2.Text"].Value)
                .MinimumLength(3).WithMessage(_localization["Validators.ContactMessage.Message3.Text"].Value)
                .MaximumLength(100).WithMessage(_localization["Validators.ContactMessage.Message4.Text"].Value);

            RuleFor(x => x.Email)
                .NotNull().WithMessage(_localization["Validators.ContactMessage.Message5.Text"].Value)
                .EmailAddress().WithMessage(_localization["Validators.ContactMessage.Message6.Text"].Value);

            RuleFor(x => x.Subject)
                .NotNull().WithMessage(_localization["Validators.ContactMessage.Message7.Text"].Value)
                .MinimumLength(3).WithMessage(_localization["Validators.ContactMessage.Message8.Text"].Value)
                .MaximumLength(150).WithMessage(_localization["Validators.ContactMessage.Message9.Text"].Value);

            RuleFor(x => x.Message)
                .NotNull().WithMessage(_localization["Validators.ContactMessage.Message10.Text"].Value)
                .MinimumLength(20).WithMessage(_localization["Validators.ContactMessage.Message11.Text"].Value)
                .MaximumLength(500).WithMessage(_localization["Validators.ContactMessage.Message12.Text"].Value);
        }
    }
}
