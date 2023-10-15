using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Models;
using VisitorBook.Core.Utilities;
using VisitorBook.UI.Languages;
using VisitorBook.UI.ViewModels;

namespace VisitorBook.UI.Controllers
{
    public class VisitedStateController : Controller
    {
        private readonly IService<State> _stateService;
        private readonly IService<Visitor> _visitorService;
        private readonly IService<VisitedState> _visitedStateService;
        private readonly IService<City> _cityService;
        private readonly IStringLocalizer<Language> _localization;

        [BindProperty]
        public VisitedStateViewModel VisitedStateViewModel { get; set; }

        public VisitedStateController(IService<State> stateService, IService<Visitor> visitorService,
        IService<VisitedState> visitedStateService, IService<City> cityService, IStringLocalizer<Language> localization)
        {
            _stateService = stateService;
            _visitorService = visitorService;
            _visitedStateService = visitedStateService;
            _cityService = cityService;
            _localization = localization;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var visitedStates = await _visitedStateService.GetAllAsync(include: u => u.Include(a => a.Visitor).Include(a => a.State).ThenInclude(b => b.City));

            return Json(new
            {
                data = visitedStates
            });
        }

        public async Task<IActionResult> AddOrEdit(int id)
        {
            VisitedStateViewModel = new VisitedStateViewModel()
            {
                VisitedState = new VisitedState(),
                StateList = new List<SelectListItem>(),
                CityList = _cityService.GetAllAsync().GetAwaiter().GetResult().ToList()
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                VisitorList = _visitorService.GetAllAsync(v => v.VisitorAddress != null).GetAwaiter().GetResult().ToList()
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name + " " + u.Surname,
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

                VisitedStateViewModel.StateList = _stateService.GetAllAsync(u => u.CityId == VisitedStateViewModel.VisitedState.CityId).GetAwaiter().GetResult().ToList()
                  .Select(u => new SelectListItem
                  {
                      Text = u.Name,
                      Value = u.Id.ToString()
                  });

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

                    return Json(new { isValid = true, message = _localization["VisitedStates.Notification.Add.Text"].Value });
                }

                else
                {
                    var visitedState = await _visitedStateService.GetAsync(u => u.Id == id);

                    visitedState.Date = VisitedStateViewModel.VisitedState.Date;
                    visitedState.VisitorId = VisitedStateViewModel.VisitedState.VisitorId;

                    var newStateWithCity = await _stateService.GetAsync(u => u.Id == VisitedStateViewModel.VisitedState.StateId, include: u => u.Include(a => a.City));

                    visitedState.CityId = newStateWithCity.City.Id;
                    visitedState.StateId = newStateWithCity.Id;

                    await _visitedStateService.UpdateAsync(visitedState);

                    return Json(new { isValid = true, message = _localization["VisitedStates.Notification.Edit.Text"].Value });
                }
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

            return Json(new { message = _localization["VisitedStates.Notification.Delete.Text"].Value });
        }
    }
}
