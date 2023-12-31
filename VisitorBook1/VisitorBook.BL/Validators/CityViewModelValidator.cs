using FluentValidation;
using VisitorBook.Core.ViewModels;

namespace VisitorBook.BL.Validators
{
    public class CityViewModelValidator : AbstractValidator<CityViewModel>
    {
        public CityViewModelValidator()
        {
            RuleFor(x => x.City.Name)
                .NotNull().WithMessage("Lütfen ad alanını boş bırakmayınız")
                .Matches("^((?![0-9]).)*$").WithMessage("İl adı rakam içeremez")
                .MinimumLength(2).WithMessage("Lütfen minimum iki karaktere sahip bir ad giriniz")
                .MaximumLength(50).WithMessage("Lütfen maksimum elli karaktere sahip bir ad giriniz");

            RuleFor(x => x.City.Code)
                .NotNull().WithMessage("Lütfen kod alanını boş bırakmayınız")
                .MinimumLength(2).WithMessage("Lütfen minimum iki karaktere sahip bir kod giriniz")
                .MaximumLength(50).WithMessage("Lütfen maksimum elli karaktere sahip bir kod giriniz");

            RuleFor(x => x.City.CountryId)
               .Must(IsCountrySelected).WithMessage("Lütfen bir ülke seçiniz");
        }

        private bool IsCountrySelected(int countryId)
        {
            if (countryId != 0)
            {
                return true;
            }

            return false;
        }
    }
}
