using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Dtos.RegisterApplicationDto;
using VisitorBook.Core.Entities;
using VisitorBook.UI.Languages;

namespace VisitorBook.UI.Validators
{
    public class RegisterApplicationCreateRequestDtoValidator : AbstractValidator<RegisterApplicationCreateRequestDto>
    {
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer<Language> _localization;

        public RegisterApplicationCreateRequestDtoValidator(UserManager<User> userManager, IStringLocalizer<Language> localization)
        {
            _userManager = userManager;
            _localization = localization;

            RuleFor(x => x.Name)
                .NotNull().WithMessage(_localization["Validators.RegisterApplication.Message1.Text"].Value)
                .Matches("^((?![0-9]).)*$").WithMessage(_localization["Validators.RegisterApplication.Message2.Text"].Value)
                .MinimumLength(3).WithMessage(_localization["Validators.RegisterApplication.Message3.Text"].Value)
                .MaximumLength(100).WithMessage(_localization["Validators.RegisterApplication.Message4.Text"].Value);

            RuleFor(x => x.Surname)
                .NotNull().WithMessage(_localization["Validators.RegisterApplication.Message5.Text"].Value)
                .Matches("^((?![0-9]).)*$").WithMessage(_localization["Validators.RegisterApplication.Message6.Text"].Value)
                .MinimumLength(3).WithMessage(_localization["Validators.RegisterApplication.Message7.Text"].Value)
                .MaximumLength(100).WithMessage(_localization["Validators.RegisterApplication.Message8.Text"].Value);

            RuleFor(x => x.Email)
                .NotNull().WithMessage(_localization["Validators.RegisterApplication.Message9.Text"].Value)
                .EmailAddress().WithMessage(_localization["Validators.RegisterApplication.Message10.Text"].Value)
                .Must(UniqueEmail).WithMessage(_localization["Validators.RegisterApplication.Message11.Text"].Value);

            RuleFor(x => x.Username)
                .NotNull().WithMessage(_localization["Validators.RegisterApplication.Message12.Text"].Value)
                .Matches("^((?![ ]).)*$").WithMessage(_localization["Validators.RegisterApplication.Message13.Text"].Value)
                .Matches("^((?![ğĞçÇşŞüÜöÖıİ]).)*$").WithMessage(_localization["Validators.RegisterApplication.Message14.Text"].Value)
                .Matches("^((?![A-Z]).)*$").WithMessage(_localization["Validators.RegisterApplication.Message15.Text"].Value)
                .Matches("^((?![0-9]).)*$").WithMessage(_localization["Validators.RegisterApplication.Message16.Text"].Value)
                .Must(UniqueUsername).WithMessage(_localization["Validators.RegisterApplication.Message17.Text"].Value);

            RuleFor(x => x.Explanation)
                .NotNull().WithMessage(_localization["Validators.RegisterApplication.Message18.Text"].Value)
                .MinimumLength(20).WithMessage(_localization["Validators.RegisterApplication.Message19.Text"].Value)
                .MaximumLength(500).WithMessage(_localization["Validators.RegisterApplication.Message20.Text"].Value);
        }

        private bool UniqueUsername(string username)
        {
            if (string.IsNullOrEmpty(username)) 
            {
                return true;
            } 

            var user = _userManager.FindByNameAsync(username).GetAwaiter().GetResult();

            if (user == null)
            {
                return true;
            }

            return false;
        }

        private bool UniqueEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return true;
            }

            var user = _userManager.FindByEmailAsync(email).GetAwaiter().GetResult();

            if (user == null)
            {
                return true;
            }

            return false;
        }
    }
}
