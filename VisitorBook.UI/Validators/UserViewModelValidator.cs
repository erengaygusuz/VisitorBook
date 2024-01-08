using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Entities;
using VisitorBook.Core.ViewModels;
using VisitorBook.UI.Languages;

namespace VisitorBook.UI.Validators
{
    public class UserViewModelValidator : AbstractValidator<UserViewModel>
    {
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer<Language> _localization;

        public UserViewModelValidator(IStringLocalizer<Language> localization, UserManager<User> userManager)
        {
            _localization = localization;
            _userManager = userManager;

            RuleFor(x => x.User.Username)
                .NotNull().WithMessage(_localization["Validators.User.Message1.Text"].Value)
                .Matches("^((?![ ]).)*$").WithMessage(_localization["Validators.User.Message2.Text"].Value)
                .Matches("^((?![ğĞçÇşŞüÜöÖıİ]).)*$").WithMessage(_localization["Validators.User.Message3.Text"].Value)
                .Matches("^((?![A-Z]).)*$").WithMessage(_localization["Validators.User.Message4.Text"].Value)
                .Matches("^((?![0-9]).)*$").WithMessage(_localization["Validators.User.Message5.Text"].Value)
                .Must((model, subModel) => IsUniqueUsername(model.User.Id, subModel)).WithMessage(_localization["Validators.User.Message6.Text"].Value);

            RuleFor(x => x.User.Email)
                .NotNull().WithMessage(_localization["Validators.User.Message7.Text"].Value)
                .EmailAddress().WithMessage(_localization["Validators.User.Message8.Text"].Value);

            RuleFor(x => x.User.Name)
                .NotNull().WithMessage(_localization["Validators.User.Message9.Text"].Value)
                .Matches("^((?![0-9]).)*$").WithMessage(_localization["Validators.User.Message10.Text"].Value)
                .MinimumLength(3).WithMessage(_localization["Validators.User.Message11.Text"].Value)
                .MaximumLength(100).WithMessage(_localization["Validators.User.Message12.Text"].Value);

            RuleFor(x => x.User.Surname)
                .NotNull().WithMessage(_localization["Validators.User.Message13.Text"].Value)
                .Matches("^((?![0-9]).)*$").WithMessage(_localization["Validators.User.Message14.Text"].Value)
                .MinimumLength(3).WithMessage(_localization["Validators.User.Message15.Text"].Value)
                .MaximumLength(100).WithMessage(_localization["Validators.User.Message16.Text"].Value);

            RuleFor(x => x.User.BirthDate)
                .NotNull().WithMessage(_localization["Validators.User.Message17.Text"].Value)
                .Must(IsValidDate).WithMessage(_localization["Validators.User.Message18.Text"].Value);

            RuleFor(x => x.User.Gender)
                .NotNull().WithMessage(_localization["Validators.User.Message19.Text"].Value);

            RuleFor(x => x.RoleId)
               .Must(IsRoleSelected).WithMessage(_localization["Validators.User.Message20.Text"].Value);
        }

        private bool IsUniqueUsername(int currentUserId, string newUsername)
        {
            if (string.IsNullOrEmpty(newUsername))
            {
                return true;
            }

            var currentUser = _userManager.Users.FirstOrDefault(x => x.Id == currentUserId);

            var others = _userManager.Users;

            if (currentUser != null)
            {
                others = _userManager.Users.Where(u => u.UserName != currentUser.UserName);
            }

            var result = others.Any(u => u.UserName == newUsername);

            if (!result)
            {
                return true;
            }

            return false;
        }

        private bool IsValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }

        private bool IsRoleSelected(int roleId)
        {
            if (roleId != 0)
            {
                return true;
            }

            return false;
        }
    }
}
