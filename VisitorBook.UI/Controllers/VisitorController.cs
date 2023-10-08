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

        public VisitorController(IService<State> stateService, IService<City> cityService, 
            IService<Visitor> visitorService)
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
            var visitors = _visitorService.GetAllAsync().GetAwaiter().GetResult().ToList()
                .Select(u => new
            {
                Id = u.Id,
                Name = u.Name,
                Surname = u.Surname,
                Gender = u.Gender.ToString(),
                BirthDate = u.BirthDate
            });

            return Json(new
            {
                data = visitors
            });
        }

        public async Task<IActionResult> AddOrEdit(int id)
        {
            VisitorViewModel = new VisitorViewModel()
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
                StateList = new List<SelectListItem>()
            };

            if (id == 0)
            {
                // create
                return View(VisitorViewModel);
            }

            else
            {
                // update
                VisitorViewModel.Visitor = await _visitorService.GetAsync(u => u.Id == id, include: u => u.Include(a => a.VisitorAddress));

                if (VisitorViewModel.Visitor.VisitorAddress != null)
                {
                    VisitorViewModel.StateList = _stateService.GetAllAsync(u => u.CityId == VisitorViewModel.Visitor.VisitorAddress.CityId).GetAwaiter().GetResult().ToList()
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   });
                }

                return View(VisitorViewModel);
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
                    var visitor = await _visitorService.GetAsync(u => u.Id == id, include: u => u.Include(a => a.VisitorAddress));

                    visitor.Name = VisitorViewModel.Visitor.Name;
                    visitor.Surname = VisitorViewModel.Visitor.Surname;
                    visitor.BirthDate = VisitorViewModel.Visitor.BirthDate;
                    visitor.Gender = VisitorViewModel.Visitor.Gender;

                    if (VisitorViewModel.Visitor.VisitorAddress != null)
                    {
                        var newStateWithCity = await _stateService.GetAsync(u => u.Id == VisitorViewModel.Visitor.VisitorAddress.StateId, include: u => u.Include(a => a.City));

                        if (visitor.VisitorAddress != null)
                        {
                            visitor.VisitorAddress.CityId = newStateWithCity.City.Id;
                            visitor.VisitorAddress.StateId = newStateWithCity.Id;
                        }

                        else
                        {
                            visitor.VisitorAddress = new VisitorAddress 
                            { 
                                StateId = newStateWithCity.Id,
                                CityId = newStateWithCity.City.Id
                            };
                        }                        
                    }

                    await _visitorService.UpdateAsync(visitor);
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
