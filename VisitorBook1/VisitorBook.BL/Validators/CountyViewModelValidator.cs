using FluentValidation;
using VisitorBook.Core.ViewModels;

namespace VisitorBook.BL.Validators
{
    public class CountyViewModelValidator : AbstractValidator<CountyViewModel>
    {
        public CountyViewModelValidator()
        {
            RuleFor(x => x.County.Name)
                .NotNull().WithMessage("Lütfen ad alanını boş bırakmayınız")
                .Matches("[a-zA-Z]").WithMessage("İlçe adı rakam içeremez")
                .MinimumLength(2).WithMessage("Lütfen minimum iki karaktere sahip bir ad giriniz")
                .MaximumLength(50).WithMessage("Lütfen maksimum elli karaktere sahip bir ad giriniz");

            RuleFor(x => x.County.Latitude)
                .NotNull().WithMessage("Lütfen enlem alanını boş bırakmayınız")
                .Must(IsValidValue).WithMessage("Lütfen geçerli enlem değeri giriniz")
                .InclusiveBetween(-90.0, 90.0).WithMessage("Enlem değeri -180 ile 180 arasında bir değer olabilir");

            RuleFor(x => x.County.Longitude)
                .NotNull().WithMessage("Lütfen boylam alanını boş bırakmayınız")
                .Must(IsValidValue).WithMessage("Lütfen geçerli bir boylam değeri giriniz")
                .InclusiveBetween(-180.0, 180.0).WithMessage("Boylam değeri -180 ile 180 arasında bir değer olabilir");

            RuleFor(x => x.County.CityId)
                .Must(IsCitySelected).WithMessage("Lütfen bir il seçiniz");
        }

        private bool IsValidValue(double value)
        {
            return !value.Equals(default(Double));
        }

        private bool IsCitySelected(int cityId)
        {
            if (cityId != 0)
            {
                return true;
            }

            return false;
        }
    }
}
