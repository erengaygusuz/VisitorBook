namespace VisitorBook.Backend.Core.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public Guid GId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
