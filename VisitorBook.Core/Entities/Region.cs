using System.ComponentModel.DataAnnotations;

namespace VisitorBook.Core.Entities
{
    public class Region : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        [MinLength(3)]
        public string Name { get; set; }
    }
}
