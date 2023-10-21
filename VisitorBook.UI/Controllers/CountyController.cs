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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var counties = await _countyService.GetAllAsync(include: u => u.Include(a => a.City));

            return Json(new
            {
                data = counties
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
