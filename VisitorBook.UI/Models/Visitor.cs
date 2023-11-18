namespace VisitorBook.UI.Models
{
    public class Visitor
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime BirthDate { get; set; }

        public string Gender { get; set; }

        public VisitorAddress VisitorAddress { get; set; }
    }
}
