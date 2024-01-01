using FluentValidation;
using VisitorBook.Core.ViewModels;

namespace VisitorBook.UI.Validators
{
    public class SubRegionViewModelValidator : AbstractValidator<SubRegionViewModel>
    {
        public SubRegionViewModelValidator()
        {
            RuleFor(x => x.SubRegion.Name)
                .NotNull().WithMessage("Lütfen ad alanını boş bırakmayınız")
                .Matches("^((?![0-9]).)*$").WithMessage("Alt bölge adı rakam içeremez")
                .MinimumLength(2).WithMessage("Lütfen minimum iki karaktere sahip bir ad giriniz")
                .MaximumLength(50).WithMessage("Lütfen maksimum elli karaktere sahip bir ad giriniz");

            RuleFor(x => x.SubRegion.RegionId)
              .Must(IsRegionSelected).WithMessage("Lütfen bir bölge seçiniz");
        }

        private bool IsRegionSelected(int regionId)
        {
            if (regionId != 0)
            {
                return true;
            }

            return false;
        }
    }
}
