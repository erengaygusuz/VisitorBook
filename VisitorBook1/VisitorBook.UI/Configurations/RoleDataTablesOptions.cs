using VisitorBook.Core.Utilities.DataTablesServerSideHelpers;

namespace VisitorBook.UI.Configurations
{
    public class RoleDataTablesOptions
    {
        private DataTablesOptions dataTablesOptions;

        public DataTablesOptions GetDataTablesOptions()
        {
            return dataTablesOptions;
        }

        public void SetDataTableOptions(HttpRequest httpRequest)
        {
            dataTablesOptions = new DataTablesOptions()
            {
                Draw = Convert.ToInt32(httpRequest.Form["draw"].FirstOrDefault()),
                Start = Convert.ToInt32(httpRequest.Form["start"].FirstOrDefault()),
                Length = Convert.ToInt32(httpRequest.Form["length"].FirstOrDefault()),
                Columns = new List<Column>
                {
                    new Column
                    {
                        Data = "Name",
                        Name = "Name",
                        Searchable = true,
                        Orderable = true,
                        Search = new Search
                        {
                            Value = httpRequest.Form["search[value]"].FirstOrDefault(),
                            Regex = ""
                        }
                    }
                },
                Search = new Search
                {
                    Value = httpRequest.Form["search[value]"].FirstOrDefault(),
                    Regex = ""
                },
                Order = new List<Order>
                {
                    new Order
                    {
                        Column = Convert.ToInt32(httpRequest.Form["order[0][column]"].FirstOrDefault()),
                        Dir = httpRequest.Form["order[0][dir]"].FirstOrDefault()
                    }
                }
            };
        }
    }
}
