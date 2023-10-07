using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Dtos;
using VisitorBook.Core.Models;
using VisitorBook.Core.Utilities;
using VisitorBook.DAL.Concrete;
using VisitorBook.UI.ViewModels;

namespace VisitorBook.UI.Controllers
{
    public class CityController : Controller
    {
        private readonly IService<City> _service;

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
            if (id == 0)
            {
                return View(new City());
            }

            else
            {
                var city = await _service.GetAsync(u => u.Id == id);

                if (city == null)
                {
                    return NotFound();
                }

                return View(city);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, City city)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    await _service.AddAsync(city);
                }

                else
                {
                    await _service.UpdateAsync(city);
                }

                return Json(new { isValid = true });
            }

            return Json(new { isValid = false, html = RazorViewConverter.GetStringFromRazorView(this, "AddOrEdit", city) });
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
