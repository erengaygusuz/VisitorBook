using FluentValidation;
using VisitorBook.Core.ViewModels;

namespace VisitorBook.BL.Validators
{
    public class VisitedCountyViewModelValidator : AbstractValidator<VisitedCountyViewModel>
    {
        public VisitedCountyViewModelValidator()
        {
            RuleFor(x => x.VisitedCounty.UserId)
               .Must(IsUserSelected).WithMessage("Lütfen bir kullanıcı seçiniz");

            RuleFor(x => x.VisitedCounty.CityId)
               .Must(IsCitySelected).WithMessage("Lütfen bir il seçiniz");

            RuleFor(x => x.VisitedCounty.CountyId)
               .Must(IsCountySelected).WithMessage("Lütfen bir ilçe seçiniz");

            RuleFor(x => x.VisitedCounty.VisitDate)
               .NotNull().WithMessage("Lütfen ziyaret tarihi alanını boş bırakmayınız")
               .Must(IsValidDate).WithMessage("Lütfen geçerli bir tarih değeri giriniz");
        }

        private bool IsValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }

        private bool IsUserSelected(int userId)
        {
            if (userId != 0)
            {
                return true;
            }

            return false;
        }

        private bool IsCitySelected(int cityId)
        {
            if (cityId != 0)
            {
                return true;
            }

            return false;
        }

        private bool IsCountySelected(int countyId)
        {
            if (countyId != 0)
            {
                return true;
            }

            return false;
        }
    }
}
