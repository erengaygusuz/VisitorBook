using Microsoft.AspNetCore.Mvc;
using VisitorBook.Core.Utilities.DataTablesServerSideHelpers;

namespace VisitorBook.UI.Areas.App.Controllers
{
    public class BaseController : Controller
    {
        protected IActionResult DataTablesResult<T>(PagedList<T> paginatedItems)
        {
            return Json(new
            {
                recordsTotal = paginatedItems.TotalCount,
                recordsFiltered = paginatedItems.TotalCount,
                data = paginatedItems
            });
        }
    }
}
