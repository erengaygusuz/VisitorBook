namespace VisitorBook.Core.Models
{
    public abstract class BaseModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
