namespace VisitorBook.Frontend.UI.Models.Outputs
{
    public class VisitorOutput
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime BirthDate { get; set; }

        public string Gender { get; set; }

        public VisitorAddressOutput? VisitorAddress { get; set; }
    }
}
