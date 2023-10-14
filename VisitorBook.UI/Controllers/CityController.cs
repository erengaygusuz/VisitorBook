using Microsoft.AspNetCore.Mvc;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Models;
using VisitorBook.Core.Utilities;

namespace VisitorBook.UI.Controllers
{
    public class CityController : Controller
    {
        private readonly IService<City> _service;

        [BindProperty]
        public City City { get; set; }

        public CityController(IService<City> service)
        {
            _service = service;
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
                }

                else
                {
                    await _service.UpdateAsync(City);
                }

                return Json(new { isValid = true });
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

            return Json(new { entityValue = cityName });
        }
    }
}
