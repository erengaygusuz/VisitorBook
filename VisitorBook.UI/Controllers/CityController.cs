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
        private readonly IService<City> _cityService;
        private readonly IStringLocalizer<Language> _localization;

        [BindProperty]
        public City City { get; set; }

        public CityController(IService<City> cityService, IStringLocalizer<Language> localization)
        {
            _cityService = cityService;
            _localization = localization;
        }

        public async Task<IActionResult> Index()
        {
            var cities = await _cityService.GetAllAsync();

            return View(cities);
        }

        [HttpPost]
        public async Task<IActionResult> GetAll()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumnIndex = Request.Form["order[0][column]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int page = (skip / pageSize) + 1;

            var cities = await _cityService.GetAllAsync(
                    page: page, pageSize: pageSize,
                    expression: (!string.IsNullOrEmpty(searchValue)) ? vc => vc.Name.Contains(searchValue) : null,
                    orderBy: (sortColumnDirection == "asc") ?
                        (o =>
                        o switch
                        {
                            _ when sortColumnIndex == "0" => o.OrderBy(s => s.Name),
                            _ when sortColumnIndex == "1" => o.OrderBy(s => s.Code),
                            _ => o.OrderBy(s => s.Name)
                        })
                         :
                        (o =>
                        o switch
                        {
                            _ when sortColumnIndex == "0" => o.OrderByDescending(s => s.Name),
                            _ when sortColumnIndex == "1" => o.OrderByDescending(s => s.Code),
                            _ => o.OrderByDescending(s => s.Name)
                        })
                );

            return Json(new
            {
                draw = draw,
                recordsFiltered = cities.Item2,
                recordsTotal = cities.Item1,
                data = cities.Item3
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
                City = await _cityService.GetAsync(u => u.Id == id);

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
                    await _cityService.AddAsync(City);

                    return Json(new { isValid = true, message = _localization["Cities.Notification.Add.Text"].Value });
                }

                else
                {
                    await _cityService.UpdateAsync(City);

                    return Json(new { isValid = true, message = _localization["Cities.Notification.Edit.Text"].Value });
                }
            }

            return Json(new { isValid = false, html = RazorViewConverter.GetStringFromRazorView(this, "AddOrEdit", City) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var city = await _cityService.GetAsync(u => u.Id == id);

            var cityName = city.Name;

            if (city != null)
            {
                await _cityService.RemoveAsync(city);
            }

            return Json(new { message = _localization["Cities.Notification.Delete.Text"].Value });
        }
    }
}
