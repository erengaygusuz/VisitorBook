using VisitorBook.Frontend.UI.Utilities.DataTablesServerSideHelpers;

namespace VisitorBook.Frontend.UI.Configurations
{
    public class VisitorDataTablesOptions
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
                        Data = "User.Name",
                        Name = "User.Name",
                        Searchable = true,
                        Orderable = true,
                        Search = new Search
                        {
                            Value = httpRequest.Form["search[value]"].FirstOrDefault(),
                            Regex = ""
                        }
                    },
                    new Column
                    {
                        Data = "User.Surname",
                        Name = "User.Surname",
                        Searchable = true,
                        Orderable = true,
                        Search = new Search
                        {
                            Value = httpRequest.Form["search[value]"].FirstOrDefault(),
                            Regex = ""
                        }
                    },
                    new Column
                    {
                        Data = "User.BirthDate",
                        Name = "User.BirthDate",
                        Searchable = true,
                        Orderable = true,
                        Search = new Search
                        {
                            Value = httpRequest.Form["search[value]"].FirstOrDefault(),
                            Regex = ""
                        }
                    },
                    new Column
                    {
                        Data = "User.Gender",
                        Name = "User.Gender",
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
                    },
                    new Order
                    {
                        Column = Convert.ToInt32(httpRequest.Form["order[0][column]"].FirstOrDefault()),
                        Dir = httpRequest.Form["order[0][dir]"].FirstOrDefault()
                    },
                    new Order
                    {
                        Column = Convert.ToInt32(httpRequest.Form["order[0][column]"].FirstOrDefault()),
                        Dir = httpRequest.Form["order[0][dir]"].FirstOrDefault()
                    },
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
