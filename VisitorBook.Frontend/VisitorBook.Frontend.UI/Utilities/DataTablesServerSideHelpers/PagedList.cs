namespace VisitorBook.Frontend.UI.Utilities.DataTablesServerSideHelpers
{
    public class PagedList<T> : List<T>
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int PagesCount { get; set; }

        private PagedList(IQueryable<T> query, DataTablesOptions paginationData)
        {
            TotalCount = query.Count();
            PageNumber = (int)Math.Ceiling(paginationData.Start / (double)paginationData.Length) + 1;
            PageSize = paginationData.Length;
            PagesCount = (int)Math.Ceiling(TotalCount / (double)PageSize);

            AddRange(query.Skip(paginationData.Start).Take(PageSize).ToList());
        }

        public static PagedList<T> Create(IQueryable<T> source, DataTablesOptions paginationData)
        {
            return new PagedList<T>(source, paginationData);
        }
    }
}
