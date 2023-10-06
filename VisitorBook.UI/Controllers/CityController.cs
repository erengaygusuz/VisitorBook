using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Models;
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
