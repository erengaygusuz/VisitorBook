namespace VisitorBook.Frontend.UI.Models.Inputs
{
    public class VisitorInput
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime BirthDate { get; set; }

        public string Gender { get; set; }

        public VisitorAddressInput? VisitorAddress { get; set; }
    }
}
