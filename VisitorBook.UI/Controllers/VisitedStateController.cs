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

        [BindProperty]
        public VisitedStateViewModel VisitedStateViewModel { get; set; }

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
            VisitedStateViewModel = new VisitedStateViewModel()
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
                return View(VisitedStateViewModel);
            }

            else
            {
                // update
                VisitedStateViewModel.VisitedState = _visitedStateService.GetAsync(u => u.Id == id).GetAwaiter().GetResult();

                return View(VisitedStateViewModel);
            }
        }

        [ActionName("AddOrEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditPost(int id)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    await _visitedStateService.AddAsync(VisitedStateViewModel.VisitedState);
                }

                else
                {
                    await _visitedStateService.UpdateAsync(VisitedStateViewModel.VisitedState);
                }

                return Json(new { isValid = true });
            }

            return Json(new { isValid = false, html = RazorViewConverter.GetStringFromRazorView(this, "AddOrEdit", VisitedStateViewModel.VisitedState) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var visitedState = await _visitedStateService.GetAsync(u => u.Id == id);

            var visitor = await _visitorService.GetAsync(u => u.Id == visitedState.VisitorId);
            var state = await _stateService.GetAsync(u => u.Id == visitedState.StateId, include: u => u.Include(a => a.City));

            var visitedStateInfo = "Visited State Record for " + visitor.Name + " " + visitor.Surname + " at " + state.Name + "/" + state.City.Name;

            if (visitedState != null)
            {
                await _visitedStateService.RemoveAsync(visitedState);
            }

            return Json(new { entityValue = visitedStateInfo });
        }
    }
}
