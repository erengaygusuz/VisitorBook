using FluentValidation;
using VisitorBook.Core.Dtos.RoleDtos;

namespace VisitorBook.UI.Validators
{
    public class RoleRequestDtoValidator : AbstractValidator<RoleRequestDto>
    {
        public RoleRequestDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Lütfen ad alanını boş bırakmayınız")
                .Matches("^((?![0-9]).)*$").WithMessage("Rol adı rakam içeremez")
                .MinimumLength(3).WithMessage("Lütfen minimum üç karaktere sahip bir ad giriniz")
                .MaximumLength(100).WithMessage("Lütfen maksimum yüz karaktere sahip bir ad giriniz");
        }
    }
}
