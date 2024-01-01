using System.ComponentModel.DataAnnotations;

namespace VisitorBook.Core.ViewModels
{
    public class FakeDataViewModel
    {
        [Required(ErrorMessage = "Lütfen alanı boş bırakmayınız")]
        [RegularExpression("^((?![A-Za-z]).)*$", ErrorMessage = "Lütfen sadece sayısal değerler giriniz")]
        public int UserAmount { get; set; }

        [Required(ErrorMessage = "Lütfen alanı boş bırakmayınız")]
        [RegularExpression("^((?![A-Za-z]).)*$", ErrorMessage = "Lütfen sadece sayısal değerler giriniz")]
        public int VisitedCountyAmount { get; set; }
    }
}
