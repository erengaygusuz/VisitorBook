using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Dtos.AuthDtos;
using VisitorBook.Core.Entities;
using VisitorBook.UI.Languages;

namespace VisitorBook.UI.Validators
{
    public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
    {
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer<Language> _localization;

        public RegisterRequestDtoValidator(UserManager<User> userManager, IStringLocalizer<Language> localization)
        {
            _userManager = userManager;
            _localization = localization;

            RuleFor(x => x.Name)
                .NotNull().WithMessage(_localization["Validators.Register.Message1.Text"].Value)
                .Matches("^((?![0-9]).)*$").WithMessage(_localization["Validators.Register.Message2.Text"].Value)
                .MinimumLength(3).WithMessage(_localization["Validators.Register.Message3.Text"].Value)
                .MaximumLength(100).WithMessage(_localization["Validators.Register.Message4.Text"].Value);

            RuleFor(x => x.Surname)
                .NotNull().WithMessage(_localization["Validators.Register.Message5.Text"].Value)
                .Matches("^((?![0-9]).)*$").WithMessage(_localization["Validators.Register.Message6.Text"].Value)
                .MinimumLength(3).WithMessage(_localization["Validators.Register.Message7.Text"].Value)
                .MaximumLength(100).WithMessage(_localization["Validators.Register.Message8.Text"].Value);

            RuleFor(x => x.Email)
                .NotNull().WithMessage(_localization["Validators.Register.Message9.Text"].Value)
                .EmailAddress().WithMessage(_localization["Validators.Register.Message10.Text"].Value);

            RuleFor(x => x.Username)
                .NotNull().WithMessage(_localization["Validators.Register.Message11.Text"].Value)
                .Matches("^((?![ ]).)*$").WithMessage(_localization["Validators.Register.Message12.Text"].Value)
                .Matches("^((?![ğĞçÇşŞüÜöÖıİ]).)*$").WithMessage(_localization["Validators.Register.Message13.Text"].Value)
                .Matches("^((?![A-Z]).)*$").WithMessage(_localization["Validators.Register.Message14.Text"].Value)
                .Matches("^((?![0-9]).)*$").WithMessage(_localization["Validators.Register.Message15.Text"].Value)
                .Must(UniqueUsername).WithMessage(_localization["Validators.Register.Message16.Text"].Value);

            RuleFor(x => x.Password)
                .NotNull().WithMessage(_localization["Validators.Register.Message17.Text"].Value);

            RuleFor(x => x.PasswordConfirm)
                .NotNull().WithMessage(_localization["Validators.Register.Message18.Text"].Value)
                .Equal(x => x.Password).WithMessage(_localization["Validators.Register.Message19.Text"].Value);
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
    }
}
