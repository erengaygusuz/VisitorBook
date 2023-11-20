using System.ComponentModel.DataAnnotations;

namespace VisitorBook.Backend.Core.Entities
{
    public class City : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string Code { get; set; }
    }
}
