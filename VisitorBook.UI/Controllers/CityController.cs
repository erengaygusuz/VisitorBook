using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Models;
using VisitorBook.Core.Utilities;
using VisitorBook.UI.Languages;

namespace VisitorBook.UI.Controllers
{
    public class CityController : Controller
    {
        private readonly IService<City> _service;
        private readonly IStringLocalizer<Language> _localization;

        [BindProperty]
        public City City { get; set; }

        public CityController(IService<City> service, IStringLocalizer<Language> localization)
        {
            _service = service;
            _localization = localization;
        }

        public async Task<IActionResult> Index()
        {
            var cities = await _service.GetAllAsync();

            return View(cities);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cities = await _service.GetAllAsync();

            return Json(new
            {
                data = cities
            });
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            City = new City();

            if (id == 0)
            {
                return View(City);
            }

            else
            {
                City = await _service.GetAsync(u => u.Id == id);

                if (City == null)
                {
                    return NotFound();
                }

                return View(City);
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
                    await _service.AddAsync(City);

                    return Json(new { isValid = true, message = _localization["Cities.Notification.Add.Text"].Value });
                }

                else
                {
                    await _service.UpdateAsync(City);

                    return Json(new { isValid = true, message = _localization["Cities.Notification.Edit.Text"].Value });
                }
            }

            return Json(new { isValid = false, html = RazorViewConverter.GetStringFromRazorView(this, "AddOrEdit", City) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var city = await _service.GetAsync(u => u.Id == id);

            var cityName = city.Name;

            if (city != null)
            {
                await _service.RemoveAsync(city);
            }

            return Json(new { message = _localization["Cities.Notification.Delete.Text"].Value });
        }
    }
}
