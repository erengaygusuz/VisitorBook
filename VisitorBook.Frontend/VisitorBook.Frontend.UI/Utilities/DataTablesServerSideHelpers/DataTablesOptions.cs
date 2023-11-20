namespace VisitorBook.Frontend.UI.Utilities.DataTablesServerSideHelpers
{
    public class DataTablesOptions
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public List<Column> Columns { get; set; } = new List<Column>();
        public Search Search { get; set; }
        public List<Order> Order { get; set; } = new List<Order>();

        public IList<Column> GetSearchableColums()
        {
            return Columns.Where(c => c.Searchable).ToList();
        }

        public IList<Column> GetSortableColumns()
        {
            return Columns.Where(c => c.Orderable).ToList();
        }
    }
}
