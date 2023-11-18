using System.ComponentModel;

namespace VisitorBook.UI.Utilities.DataTablesServerSideHelpers
{
    public class Order
    {
        public int Column { get; set; }
        public string Dir { get; set; }

        public ListSortDirection GetSortDirection()
        {
            if (string.IsNullOrWhiteSpace(Dir))
            {
                return ListSortDirection.Ascending;
            }

            return Dir.Equals("asc", StringComparison.CurrentCultureIgnoreCase) ? ListSortDirection.Ascending : ListSortDirection.Descending;
        }
    }
}
