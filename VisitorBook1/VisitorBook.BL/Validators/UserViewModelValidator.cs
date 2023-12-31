using FluentValidation;
using Microsoft.AspNetCore.Identity;
using VisitorBook.Core.Entities;
using VisitorBook.Core.ViewModels;

namespace VisitorBook.BL.Validators
{
    public class UserViewModelValidator : AbstractValidator<UserViewModel>
    {
        private readonly UserManager<User> _userManager;

        public UserViewModelValidator(UserManager<User> userManager)
        {
            _userManager = userManager;

            RuleFor(x => x.User.Username)
                .NotNull().WithMessage("Lütfen kullanıcı adı alanını boş bırakmayınız")
                .Matches("^((?![ ]).)*$").WithMessage("Kullanıcı adınız boşluk içeremez")
                .Matches("^((?![ğĞçÇşŞüÜöÖıİ]).)*$").WithMessage("Kullanıcı adınız türkçe karakter içeremez")
                .Matches("^((?![A-Z]).)*$").WithMessage("Kullanıcı adınız büyük harf içeremez")
                .Matches("^((?![0-9]).)*$").WithMessage("Kullanıcı adınız rakam içeremez")
                .Must(UniqueUsername).WithMessage("Bu kullanıcı adı daha önceden alınmış. Lütfen farklı bir kullanıcı adı seçiniz");

            RuleFor(x => x.User.Email)
                .NotNull().WithMessage("Lütfen email alanını boş bırakmayınız")
                .EmailAddress().WithMessage("Lütfen geçerli bir e-posta giriniz");

            RuleFor(x => x.User.Name)
                .NotNull().WithMessage("Lütfen ad alanını boş bırakmayınız")
                .Matches("^((?![0-9]).)*$").WithMessage("Ad rakam içeremez")
                .MinimumLength(3).WithMessage("Lütfen minimum üç karaktere sahip bir ad giriniz")
                .MaximumLength(100).WithMessage("Lütfen maksimum yüz karaktere sahip bir ad giriniz");

            RuleFor(x => x.User.Surname)
                .NotNull().WithMessage("Lütfen soyad alanını boş bırakmayınız")
                .Matches("^((?![0-9]).)*$").WithMessage("Soyad rakam içeremez")
                .MinimumLength(3).WithMessage("Lütfen minimum üç karaktere sahip bir soyad giriniz")
                .MaximumLength(100).WithMessage("Lütfen maksimum yüz karaktere sahip bir soyad giriniz");

            RuleFor(x => x.User.BirthDate)
                .NotNull().WithMessage("Lütfen doğum tarihi alanını boş bırakmayınız")
                .Must(IsValidDate).WithMessage("Lütfen geçerli bir tarih değeri giriniz");

            RuleFor(x => x.User.Gender)
                .NotNull().WithMessage("Lütfen cinsiyet alanını boş bırakmayınız");

            RuleFor(x => x.RoleId)
               .Must(IsRoleSelected).WithMessage("Lütfen bir rol seçiniz");
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
