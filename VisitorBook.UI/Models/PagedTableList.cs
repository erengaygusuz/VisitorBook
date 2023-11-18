namespace VisitorBook.UI.Models
{
    public class PagedTableList<T>
    {
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<T> Data { get; set; }
    }
}
