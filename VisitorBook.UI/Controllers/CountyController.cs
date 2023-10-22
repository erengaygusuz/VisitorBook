using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Models;
using VisitorBook.Core.Utilities;
using VisitorBook.UI.Languages;
using VisitorBook.UI.ViewModels;

namespace VisitorBook.UI.Controllers
{
    public class CountyController : Controller
    {
        private readonly IService<County> _countyService;
        private readonly IService<City> _cityService;
        private readonly IStringLocalizer<Language> _localization;

        [BindProperty]
        public CountyViewModel CountyViewModel { get; set; }

        public CountyController(IService<County> countyService, IService<City> cityService, IStringLocalizer<Language> localization)
        {
            _countyService = countyService;
            _cityService = cityService;
            _localization = localization;
        }

        public IActionResult Index()
        {
            return View();
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

            var counties = await _countyService.GetAllAsync(
                    page: page, pageSize: pageSize,
                    expression: (!string.IsNullOrEmpty(searchValue)) ? vc => vc.Name.Contains(searchValue) : null,
                    include: u => u.Include(a => a.City),
                    orderBy: (sortColumnDirection == "asc") ?
                        (o =>
                        o switch
                        {
                            _ when sortColumnIndex == "0" => o.OrderBy(s => s.Name),
                            _ when sortColumnIndex == "1" => o.OrderBy(s => s.City.Name),
                            _ when sortColumnIndex == "2" => o.OrderBy(s => s.Latitude),
                            _ when sortColumnIndex == "3" => o.OrderBy(s => s.Longitude),
                            _ => o.OrderBy(s => s.Name)
                        })
                         :
                        (o =>
                        o switch
                        {
                            _ when sortColumnIndex == "0" => o.OrderByDescending(s => s.Name),
                            _ when sortColumnIndex == "1" => o.OrderByDescending(s => s.City.Name),
                            _ when sortColumnIndex == "2" => o.OrderByDescending(s => s.Latitude),
                            _ when sortColumnIndex == "3" => o.OrderByDescending(s => s.Longitude),
                            _ => o.OrderByDescending(s => s.Name)
                        })
                );

            return Json(new
            {
                draw = draw,
                recordsFiltered = counties.Item2,
                recordsTotal = counties.Item1,
                data = counties.Item3
            });
        }

        public async Task<IActionResult> GetAllByCity(int cityId)
        {
            var counties = await _countyService.GetAllAsync(u => u.CityId == cityId);

            return Json(new
            {
                data = counties
            });
        }

        public IActionResult AddOrEdit(int id)
        {
            CountyViewModel = new CountyViewModel()
            {
                CityList = _cityService.GetAllAsync().GetAwaiter().GetResult().ToList()
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                County = new County()
            };

            if (id == 0)
            {
                // create
                return View(CountyViewModel);
            }

            else
            {
                // update
                CountyViewModel.County = _countyService.GetAsync(u => u.Id == id).GetAwaiter().GetResult();

                return View(CountyViewModel);
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
                    await _countyService.AddAsync(CountyViewModel.County);

                    return Json(new { isValid = true, message = _localization["Countys.Notification.Add.Text"].Value });
                }

                else
                {
                    await _countyService.UpdateAsync(CountyViewModel.County);

                    return Json(new { isValid = true, message = _localization["Countys.Notification.Edit.Text"].Value });
                }
            }

            return Json(new { isValid = false, html = RazorViewConverter.GetStringFromRazorView(this, "AddOrEdit", CountyViewModel.County) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var county = await _countyService.GetAsync(u => u.Id == id);

            var countyName = county.Name;

            if (county != null)
            {
                await _countyService.RemoveAsync(county);
            }

            return Json(new { message = _localization["Countys.Notification.Delete.Text"].Value });
        }
    }
}
