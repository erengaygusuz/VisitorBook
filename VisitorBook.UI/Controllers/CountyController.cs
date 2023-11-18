using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using VisitorBook.UI.Attributes;
using VisitorBook.Core.Dtos.CountyDtos;
using VisitorBook.Core.Utilities;
using VisitorBook.UI.Configurations;
using VisitorBook.UI.Languages;
using VisitorBook.UI.Services;
using VisitorBook.UI.ViewModels;
using VisitorBook.UI.Models;

namespace VisitorBook.UI.Controllers
{
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
                County = new County()
            };

            return View(countyViewModel);
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(Guid id)
        {
            var county = await _countyApiService.GetByIdAsync<CountyResponseDto>(id);

            var cities = await _cityApiService.GetAllAsync();

            var countyViewModel = new CountyViewModel()
            {
                CityList = (cities)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                County = county
            };

            return View(countyViewModel);
        }

        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(CountyRequestDto countyRequestDto)
        {
            if (ModelState.IsValid)
            {
                await _countyApiService.AddAsync(countyRequestDto);

                return Json(new { isValid = true, message = _localization["Counties.Notification.Add.Text"].Value });
            }

            var cities = await _cityApiService.GetAllAsync();

            var countyViewModel = new CountyViewModel()
            {
                CityList = (cities)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   })
            };

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Add", countyViewModel) });
        }

        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(Guid id, CountyRequestDto countyRequestDto)
        {
            if (ModelState.IsValid)
            {
                await _countyApiService.UpdateAsync(id, countyRequestDto);

                return Json(new { isValid = true, message = _localization["Counties.Notification.Edit.Text"].Value });
            }

            var cities = await _cityApiService.GetAllAsync();

            var countyViewModel = new CountyViewModel()
            {
                CityList = (cities)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   })
            };

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
