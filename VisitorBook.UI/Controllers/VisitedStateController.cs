using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Models;
using VisitorBook.Core.Utilities;
using VisitorBook.UI.ViewModels;

namespace VisitorBook.UI.Controllers
{
    public class VisitedStateController : Controller
    {
        private readonly IService<State> _stateService;
        private readonly IService<Visitor> _visitorService;
        private readonly IService<VisitedState> _visitedStateService;

        public VisitedStateController(IService<State> stateService, IService<Visitor> visitorService,
        IService<VisitedState> visitedStateService)
        {
            _stateService = stateService;
            _visitorService = visitorService;
            _visitedStateService = visitedStateService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var visitedStates = await _visitedStateService.GetAllAsync(include: u => u.Include(a => a.Visitor).Include(a => a.State));

            return Json(new
            {
                data = visitedStates
            });
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

            if (id == 0)
            {
                // create
                return View(visitedStateViewModel);
            }

            else
            {
                // update
                visitedStateViewModel.VisitedState = _visitedStateService.GetAsync(u => u.Id == id).GetAwaiter().GetResult();

                return View(visitedStateViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, VisitedState visitedState)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    await _visitedStateService.AddAsync(visitedState);
                }

                else
                {
                    await _visitedStateService.UpdateAsync(visitedState);
                }

                return Json(new { isValid = true });
            }

            return Json(new { isValid = false, html = RazorViewConverter.GetStringFromRazorView(this, "AddOrEdit", visitedState) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var visitedState = await _visitedStateService.GetAsync(u => u.Id == id);

            var visitedStateInfo = "Visited State Record for " + visitedState.Visitor.Name + " at " + visitedState.State.Name;

            if (visitedState != null)
            {
                await _visitedStateService.RemoveAsync(visitedState);
            }

            return Json(new { entityValue = visitedStateInfo });
        }
    }
}
