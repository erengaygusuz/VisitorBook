using FluentValidation;
using VisitorBook.Core.ViewModels;

namespace VisitorBook.BL.Validators
{
    public class CountryViewModelValidator : AbstractValidator<CountryViewModel>
    {
        public CountryViewModelValidator()
        {
            RuleFor(x => x.Country.Name)
                .NotNull().WithMessage("Lütfen ad alanını boş bırakmayınız")
                .Matches("^((?![0-9]).)*$").WithMessage("Ülke adı rakam içeremez")
                .MinimumLength(2).WithMessage("Lütfen minimum iki karaktere sahip bir ad giriniz")
                .MaximumLength(50).WithMessage("Lütfen maksimum elli karaktere sahip bir ad giriniz");

            RuleFor(x => x.Country.Code)
                .NotNull().WithMessage("Lütfen kod alanını boş bırakmayınız")
                .MinimumLength(2).WithMessage("Lütfen minimum iki karaktere sahip bir kod giriniz")
                .MaximumLength(50).WithMessage("Lütfen maksimum elli karaktere sahip bir kod giriniz");

            RuleFor(x => x.Country.SubRegionId)
               .Must(IsSubRegionSelected).WithMessage("Lütfen bir alt bölge seçiniz");
        }

        private bool IsSubRegionSelected(int subRegionId)
        {
            if (subRegionId != 0)
            {
                return true;
            }

            return false;
        }
    }
}
