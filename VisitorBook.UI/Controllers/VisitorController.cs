using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Models;
using VisitorBook.UI.ViewModels;

namespace VisitorBook.UI.Controllers
{
    public class VisitorController : Controller
    {
        private readonly IService<State> _stateService;
        private readonly IService<City> _cityService;
        private readonly IService<Visitor> _visitorService;

        public VisitorController(IService<State> stateService, IService<City> cityService, IService<Visitor> visitorService)
        {
            _stateService = stateService;
            _cityService = cityService;
            _visitorService = visitorService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var visitors = await _visitorService.GetAllAsync();

            return Json(new
            {
                data = visitors
            });
        }

        public IActionResult AddOrEdit(int id = 0)
        {
            VisitorViewModel visitorViewModel = new VisitorViewModel()
            {
                Visitor = new Visitor(),
                VisitorAddress = new VisitorAddress(),
                GenderList = new List<Gender> { Gender.Man, Gender.Woman }
                    .Select(u => new SelectListItem
                    {
                        Text = u.ToString(),
                        Value = u.ToString()
                    }),
                CityList = _cityService.GetAllAsync().GetAwaiter().GetResult().ToList()
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                StateList = _stateService.GetAllAsync().GetAwaiter().GetResult().ToList()
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   })
            };

            if (id == null || id == 0)
            {
                // create
                return View(visitorViewModel);
            }

            else
            {
                // update

                return View(visitorViewModel);
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
