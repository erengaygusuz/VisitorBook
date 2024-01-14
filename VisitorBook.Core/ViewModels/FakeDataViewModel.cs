using System.ComponentModel.DataAnnotations;

namespace VisitorBook.Core.ViewModels
{
    public class FakeDataViewModel
    {
        [Required]
        [Range(1, 99)]
        public int UserAmount { get; set; }

        [Required]
        [Range(1, 99)]
        public int VisitedCountyAmount { get; set; }
    }
}
