using Microsoft.AspNetCore.Mvc;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Models;

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

            return View();
        }

        public IActionResult AddOrEdit(int id = 0)
        {
            return View();
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
