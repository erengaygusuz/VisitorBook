using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Models;
using VisitorBook.Core.Utilities;
using VisitorBook.UI.ViewModels;

namespace VisitorBook.UI.Controllers
{
    public class VisitorController : Controller
    {
        private readonly IService<State> _stateService;
        private readonly IService<City> _cityService;
        private readonly IService<Visitor> _visitorService;

        [BindProperty]
        public VisitorViewModel VisitorViewModel { get; set; }

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

        public async Task<IActionResult> AddOrEdit(int id)
        {
            VisitorViewModel visitorViewModel = new VisitorViewModel()
            {
                Visitor = new Visitor(),
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

            if (id == 0)
            {
                // create
                return View(visitorViewModel);
            }

            else
            {
                // update
                visitorViewModel.Visitor = await _visitorService.GetAsync(u => u.Id == id, include: u => u.Include(a => a.VisitorAddress));

                return View(visitorViewModel);
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
                    await _visitorService.AddAsync(VisitorViewModel.Visitor);
                }

                else
                {
                    await _visitorService.UpdateAsync(VisitorViewModel.Visitor);
                }

                return Json(new { isValid = true });
            }

            return Json(new { isValid = false, html = RazorViewConverter.GetStringFromRazorView(this, "AddOrEdit", VisitorViewModel.Visitor) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var visitor = await _visitorService.GetAsync(u => u.Id == id);

            var visitorName = visitor.Name + " " + visitor.Surname;

            if (visitor != null)
            {
                await _visitorService.RemoveAsync(visitor);
            }

            return Json(new { entityValue = visitorName });
        }
    }
}
