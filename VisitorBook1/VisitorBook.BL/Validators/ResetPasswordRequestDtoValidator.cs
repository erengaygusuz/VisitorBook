using FluentValidation;
using VisitorBook.Core.Dtos.AuthDtos;
namespace VisitorBook.BL.Validators
{
    public class ResetPasswordRequestDtoValidator : AbstractValidator<ResetPasswordRequestDto>
    {
        public ResetPasswordRequestDtoValidator()
        {
            RuleFor(x => x.Password)
                .NotNull().WithMessage("Lütfen şifre alanını boş bırakmayınız");

            RuleFor(x => x.PasswordConfirm)
                .NotNull().WithMessage("Lütfen şifre tekrar alanını boş bırakmayınız")
                .Equal(x => x.Password).WithMessage("Şifreleriniz birbiriyle uyuşmuyor");
        }
    }
}
