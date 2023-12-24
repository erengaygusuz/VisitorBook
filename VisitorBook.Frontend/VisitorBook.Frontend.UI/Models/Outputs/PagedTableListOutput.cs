namespace VisitorBook.Frontend.UI.Models.Outputs
{
    public class PagedTableListOutput<T>
    {
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<T> Data { get; set; }
    }
}
