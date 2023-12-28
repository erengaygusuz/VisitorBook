using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VisitorBook.Backend.Core.Utilities.DataTablesServerSideHelpers;

namespace VisitorBook.Backend.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IMapper _mapper;

        public BaseController(IMapper mapper)
        {
            _mapper = mapper;
        }

        protected OkObjectResult DataTablesResult<T>(PagedList<T> paginatedItems)
        {
            return Ok(new
            {
                recordsTotal = paginatedItems.TotalCount,
                recordsFiltered = paginatedItems.TotalCount,
                data = paginatedItems
            });
        }
    }
}
