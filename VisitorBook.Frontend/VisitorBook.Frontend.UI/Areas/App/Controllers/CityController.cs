using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using VisitorBook.Frontend.UI.Attributes;
using VisitorBook.Frontend.UI.Utilities;
using VisitorBook.Frontend.UI.Configurations;
using VisitorBook.Frontend.UI.Languages;
using VisitorBook.Frontend.UI.Services;
using VisitorBook.Frontend.UI.Models.Inputs;
using Microsoft.AspNetCore.Mvc.Rendering;
using VisitorBook.Frontend.UI.ViewModels;

namespace VisitorBook.Frontend.UI.Area.App.Controllers
{
    [Area("App")]
    public class CityController : Controller
    {
        private readonly IStringLocalizer<Language> _localization;
        private readonly RazorViewConverter _razorViewConverter;
        private readonly CityApiService _cityApiService;
        private readonly CountryApiService _countryApiService;
        private readonly CityDataTablesOptions _cityDataTableOptions;

        public CityController(RazorViewConverter razorViewConverter, 
            IStringLocalizer<Language> localization, CityApiService cityApiService, CityDataTablesOptions cityDataTableOptions, CountryApiService countryApiService)
        {
            _cityApiService = cityApiService;
            _localization = localization;
            _razorViewConverter = razorViewConverter;
            _cityDataTableOptions = cityDataTableOptions;
            _countryApiService = countryApiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAll()
        {
            _cityDataTableOptions.SetDataTableOptions(Request);

            var result = await _cityApiService.GetTableData(_cityDataTableOptions.GetDataTablesOptions(), Request.Cookies["X-Access-Token"]);

            return Json(new
            {
                recordsFiltered = result.RecordsFiltered,
                recordsTotal = result.RecordsTotal,
                data = result.Data
            });
        }

        public async Task<IActionResult> GetAllByCountry(int countryId)
        {
            var cities = await _cityApiService.GetAllByCountryAsync(countryId);

            return Json(new
            {
                data = cities
            });
        }

        [NoDirectAccess]
        public async Task<IActionResult> Add()
        {
            var countries = await _countryApiService.GetAllAsync();

            var cityViewModel = new CityViewModel()
            {
                CountryList = (countries)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                City = new CityInput()
            };

            return View(cityViewModel);
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id)
        {
            var city = await _cityApiService.GetByIdAsync(id);

            var countries = await _countryApiService.GetAllAsync();

            var cityViewModel = new CityViewModel()
            {
                CountryList = (countries)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                City = new CityInput()
                {
                    Name = city.Name,
                    Code = city.Code,
                    CountryId = city.Country.Id
                }
            };

            ViewData["Id"] = id;

            return View(cityViewModel);
        }

        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(CityViewModel cityViewModel)
        {
            if (ModelState.IsValid)
            {
                await _cityApiService.AddAsync(cityViewModel.City);

                return Json(new { isValid = true, message = _localization["Cities.Notification.Add.Text"].Value });
            }

            var countries = await _countryApiService.GetAllAsync();

            cityViewModel.CountryList = (countries)
                  .Select(u => new SelectListItem
                  {
                      Text = u.Name,
                      Value = u.Id.ToString()
                  });

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Add", cityViewModel) });
        }

        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id, CityViewModel cityViewModel)
        {
            if (ModelState.IsValid)
            {
                await _cityApiService.UpdateAsync(id, cityViewModel.City);

                return Json(new { isValid = true, message = _localization["Cities.Notification.Edit.Text"].Value });
            }

            var countries = await _countryApiService.GetAllAsync();

            cityViewModel.CountryList = (countries)
                  .Select(u => new SelectListItem
                  {
                      Text = u.Name,
                      Value = u.Id.ToString()
                  });

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Edit", cityViewModel) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _cityApiService.RemoveAsync(id);

            if (result)
            {
                return Json(new { message = _localization["Cities.Notification.SuccessfullDelete.Text"].Value });
            }

            return BadRequest(new { message = _localization["Cities.Notification.UnSuccessfullDelete.Text"].Value });
        }
    }
}
