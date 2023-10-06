using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Models;
using VisitorBook.UI.ViewModels;

namespace VisitorBook.UI.Controllers
{
    public class VisitedStateController : Controller
    {
        private readonly IService<State> _stateService;
        private readonly IService<Visitor> _visitorService;

        public VisitedStateController(IService<State> stateService, IService<Visitor> visitorService)
        {
            _stateService = stateService;
            _visitorService = visitorService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddOrEdit(int id)
        {
            VisitedStateViewModel visitedStateViewModel = new VisitedStateViewModel()
            {
                VisitedState = new VisitedState(),
                StateList = _stateService.GetAllAsync().GetAwaiter().GetResult().ToList()
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                VisitorList = _visitorService.GetAllAsync().GetAwaiter().GetResult().ToList()
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   })
            };

            if (id == null || id == 0)
            {
                // create
                return View(visitedStateViewModel);
            }

            else
            {
                // update

                return View(visitedStateViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            return View();
        }
    }
}
