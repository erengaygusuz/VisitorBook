using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Models;
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

            if (id == null || id == 0)
            {
                // create
                return View(stateViewModel);
            }

            else
            {
                // update
                stateViewModel.State = _stateService.GetAsync(id).GetAwaiter().GetResult();

                return View(stateViewModel);
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
