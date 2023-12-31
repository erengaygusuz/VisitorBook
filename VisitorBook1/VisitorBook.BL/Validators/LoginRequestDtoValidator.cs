using FluentValidation;
using VisitorBook.Core.Dtos.AuthDtos;

namespace VisitorBook.BL.Validators
{
    public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestDtoValidator()
        {
            RuleFor(x => x.Email).NotNull().WithMessage("Lütfen geçerli bir e-posta giriniz");
            RuleFor(x => x.Password).NotNull().WithMessage("Lütfen geçerli bir şifre giriniz");
        }
    }
}
