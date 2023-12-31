using FluentValidation;
using Microsoft.AspNetCore.Identity;
using VisitorBook.Core.Dtos.AuthDtos;
using VisitorBook.Core.Entities;

namespace VisitorBook.BL.Validators
{
    public class ForgotPasswordRequestDtoValidator : AbstractValidator<ForgotPasswordRequestDto>
    {
        private readonly UserManager<User> _userManager;

        public ForgotPasswordRequestDtoValidator(UserManager<User> userManager)
        {
            _userManager = userManager;

            RuleFor(x => x.Email)
                .NotNull().WithMessage("Lütfen email alanını boş bırakmayınız")
                .EmailAddress().WithMessage("Lütfen geçerli bir e-posta giriniz")
                .Must(NotExistEmail).WithMessage("Bu e-posta adresi kayıtlarımızda bulunmamaktadır");
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
