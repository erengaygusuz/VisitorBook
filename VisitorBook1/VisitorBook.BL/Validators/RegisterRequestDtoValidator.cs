using FluentValidation;
using Microsoft.AspNetCore.Identity;
using VisitorBook.Core.Dtos.AuthDtos;
using VisitorBook.Core.Entities;

namespace VisitorBook.BL.Validators
{
    public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
    {
        private readonly UserManager<User> _userManager;

        public RegisterRequestDtoValidator(UserManager<User> userManager)
        {
            _userManager = userManager;

            RuleFor(x => x.Name)
                .NotNull().WithMessage("Lütfen ad alanını boş bırakmayınız")
                .MinimumLength(3).WithMessage("Lütfen minimum üç karaktere sahip bir ad giriniz")
                .MaximumLength(100).WithMessage("Lütfen maksimum yüz karaktere sahip bir ad giriniz");

            RuleFor(x => x.Surname)
                .NotNull().WithMessage("Lütfen soyad alanını boş bırakmayınız")
                .MinimumLength(3).WithMessage("Lütfen minimum üç karaktere sahip bir soyad giriniz")
                .MaximumLength(100).WithMessage("Lütfen maksimum yüz karaktere sahip bir soyad giriniz");

            RuleFor(x => x.Email)
                .NotNull().WithMessage("Lütfen email alanını boş bırakmayınız")
                .EmailAddress().WithMessage("Lütfen geçerli bir e-posta giriniz");

            RuleFor(x => x.Username)
                .NotNull().WithMessage("Lütfen kullanıcı adı alanını boş bırakmayınız")
                .Matches("^((?![ ]).)*$").WithMessage("Kullanıcı adınız boşluk içeremez")
                .Matches("^((?![ğĞçÇşŞüÜöÖıİ]).)*$").WithMessage("Kullanıcı adınız türkçe karakter içeremez")
                .Matches("^((?![A-Z]).)*$").WithMessage("Kullanıcı adınız büyük harf içeremez")
                .Matches("^((?![0-9]).)*$").WithMessage("Kullanıcı adınız rakam içeremez")
                .Must(UniqueUsername).WithMessage("Bu kullanıcı adı daha önceden alınmış. Lütfen farklı bir kullanıcı adı seçiniz");

            RuleFor(x => x.Password)
                .NotNull().WithMessage("Lütfen şifre alanını boş bırakmayınız");

            RuleFor(x => x.PasswordConfirm)
                .NotNull().WithMessage("Lütfen şifre tekrar alanını boş bırakmayınız")
                .Equal(x => x.Password).WithMessage("Şifreleriniz birbiriyle uyuşmuyor");
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
