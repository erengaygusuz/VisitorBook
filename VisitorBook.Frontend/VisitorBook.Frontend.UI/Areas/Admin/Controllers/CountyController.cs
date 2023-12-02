using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using VisitorBook.Frontend.UI.Attributes;
using VisitorBook.Frontend.UI.Utilities;
using VisitorBook.Frontend.UI.Configurations;
using VisitorBook.Frontend.UI.Languages;
using VisitorBook.Frontend.UI.Services;
using VisitorBook.Frontend.UI.ViewModels;
using VisitorBook.Frontend.UI.Models.Inputs;

namespace VisitorBook.Frontend.UI.Area.Controllers
{
    [Area("Admin")]
    public class CountyController : Controller
    {
        private readonly IStringLocalizer<Language> _localization;
        private readonly RazorViewConverter _razorViewConverter;
        private readonly CountyApiService _countyApiService;
        private readonly CityApiService _cityApiService;
        private readonly CountyDataTablesOptions _countyDataTableOptions;

        public CountyController(CountyApiService countyApiService, CityApiService cityApiService, IStringLocalizer<Language> localization, 
            RazorViewConverter razorViewConverter, CountyDataTablesOptions countyDataTableOptions)
        {
            _countyApiService = countyApiService;
            _cityApiService = cityApiService;
            _localization = localization;
            _razorViewConverter = razorViewConverter;
            _countyDataTableOptions = countyDataTableOptions;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAll()
        {
            _countyDataTableOptions.SetDataTableOptions(Request);

            var result = await _countyApiService.GetTableData(_countyDataTableOptions.GetDataTablesOptions());

            return Json(new
            {
                recordsFiltered = result.RecordsFiltered,
                recordsTotal = result.RecordsTotal,
                data = result.Data
            });
        }

        public async Task<IActionResult> GetAllByCity(Guid cityId)
        {
            var counties = await _countyApiService.GetAllByCityAsync(cityId);

            return Json(new
            {
                data = counties
            });
        }

        [NoDirectAccess]
        public async Task<IActionResult> Add()
        {
            var cities = await _cityApiService.GetAllAsync();

            var countyViewModel = new CountyViewModel()
            {
                CityList = (cities)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                County = new CountyInput()
            };

            return View(countyViewModel);
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(Guid id)
        {
            var county = await _countyApiService.GetByIdAsync(id);

            var cities = await _cityApiService.GetAllAsync();

            var countyViewModel = new CountyViewModel()
            {
                CityList = (cities)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                County = new CountyInput()
                {
                    Name = county.Name,
                    Latitude = county.Latitude,
                    Longitude = county.Longitude,
                    CityId = county.City.Id
                }
            };

            ViewData["Id"] = id;

            return View(countyViewModel);
        }

        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(CountyViewModel countyViewModel)
        {
            if (ModelState.IsValid)
            {
                await _countyApiService.AddAsync(countyViewModel.County);

                return Json(new { isValid = true, message = _localization["Counties.Notification.Add.Text"].Value });
            }

            var cities = await _cityApiService.GetAllAsync();

            countyViewModel.CityList = (cities)
                  .Select(u => new SelectListItem
                  {
                      Text = u.Name,
                      Value = u.Id.ToString()
                  });

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Add", countyViewModel) });
        }

        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(Guid id, CountyViewModel countyViewModel)
        {
            if (ModelState.IsValid)
            {
                await _countyApiService.UpdateAsync(id, countyViewModel.County);

                return Json(new { isValid = true, message = _localization["Counties.Notification.Edit.Text"].Value });
            }

            var cities = await _cityApiService.GetAllAsync();

            countyViewModel.CityList = (cities)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   });

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Edit", countyViewModel) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _countyApiService.RemoveAsync(id);

            if (result)
            {
                return Json(new { message = _localization["Counties.Notification.SuccessfullDelete.Text"].Value });
            }

            return BadRequest(new { message = _localization["Counties.Notification.UnSuccessfullDelete.Text"].Value });
        }
    }
}
