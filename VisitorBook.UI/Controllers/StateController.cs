using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Models;
using VisitorBook.Core.Utilities;
using VisitorBook.UI.ViewModels;

namespace VisitorBook.UI.Controllers
{
    public class StateController : Controller
    {
        private readonly IService<State> _stateService;
        private readonly IService<City> _cityService;

        public StateController(IService<State> stateService, IService<City> cityService)
        {
            _stateService = stateService;
            _cityService = cityService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var states = await _stateService.GetAllAsync(include: u => u.Include(a => a.City));

            return Json(new
            {
                data = states
            });
        }

        public async Task<IActionResult> GetAllByCity(int cityId)
        {
            var states = await _stateService.GetAllAsync(u => u.CityId == cityId);

            return Json(new
            {
                data = states
            });
        }

        public IActionResult AddOrEdit(int id)
        {
            StateViewModel stateViewModel = new StateViewModel()
            {
                CityList = _cityService.GetAllAsync().GetAwaiter().GetResult().ToList()
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                State = new State()
            };

            if (id == 0)
            {
                // create
                return View(stateViewModel);
            }

            else
            {
                // update
                stateViewModel.State = _stateService.GetAsync(u => u.Id == id).GetAwaiter().GetResult();

                return View(stateViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, State state)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    await _stateService.AddAsync(state);
                }

                else
                {
                    await _stateService.UpdateAsync(state);
                }

                return Json(new { isValid = true });
            }

            return Json(new { isValid = false, html = RazorViewConverter.GetStringFromRazorView(this, "AddOrEdit", state) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var state = await _stateService.GetAsync(u => u.Id == id);

            var stateName = state.Name;

            if (state != null)
            {
                await _stateService.RemoveAsync(state);
            }

            return Json(new { entityValue = stateName });
        }
    }
}
