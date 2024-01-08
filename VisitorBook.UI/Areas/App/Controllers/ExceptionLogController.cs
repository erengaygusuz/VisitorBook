using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Constants;
using VisitorBook.Core.Entities;

namespace VisitorBook.UI.Areas.App.Controllers
{
    [Authorize]
    [Area("App")]
    public class ExceptionLogController : Controller
    {
        private readonly IRepository<ExceptionLog> _exceptionLogRepository;

        public ExceptionLogController(IRepository<ExceptionLog> exceptionLogRepository)
        {
            _exceptionLogRepository = exceptionLogRepository;
        }

        [Authorize(Permissions.ExceptionLogManagement.View)]
        [HttpGet]
        public IActionResult Index()
        {
            var exceptionLogs = _exceptionLogRepository.GetAll().OrderByDescending(x => x.CreatedDate);

            return View(exceptionLogs);
        }

        [Authorize(Permissions.ExceptionLogManagement.View)]
        [HttpGet]
        public async Task<IActionResult> Log(int id)
        {
            var exceptionLog = await _exceptionLogRepository.GetAsync(x => x.Id == id);

            return View(exceptionLog);
        }
    }
}
