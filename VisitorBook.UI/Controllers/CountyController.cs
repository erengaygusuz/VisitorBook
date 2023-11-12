using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Attributes;
using VisitorBook.Core.Dtos.CountyDtos;
using VisitorBook.Core.Utilities;
using VisitorBook.UI.Configurations;
using VisitorBook.UI.Languages;
using VisitorBook.UI.Services;
using VisitorBook.UI.ViewModels;

namespace VisitorBook.UI.Controllers
{
    public class CountyController : Controller
    {
        private readonly IStringLocalizer<Language> _localization;
        private readonly RazorViewConverter _razorViewConverter;
        private readonly CountyApiService _countyApiService;
        private readonly CityApiService _cityApiService;
        private readonly CountyDataTableOptions _countyDataTableOptions;

        public CountyController(CountyApiService countyApiService, CityApiService cityApiService, IStringLocalizer<Language> localization, 
            RazorViewConverter razorViewConverter, CountyDataTableOptions countyDataTableOptions)
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

            var countyAddViewModel = new CountyAddViewModel()
            {
                CityList = (cities)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                CountyAddRequestDto = new CountyAddRequestDto()
            };

            return View(countyAddViewModel);
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(Guid id)
        {
            var county = await _countyApiService.GetByIdAsync<CountyUpdateRequestDto>(id);

            var cities = await _cityApiService.GetAllAsync();

            var countyUpdateViewModel = new CountyUpdateViewModel()
            {
                CityList = (cities)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                CountyUpdateRequestDto = county
            };

            return View(countyUpdateViewModel);
        }

        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(CountyAddRequestDto countyAddRequestDto)
        {
            if (ModelState.IsValid)
            {
                await _countyApiService.AddAsync(countyAddRequestDto);

                return Json(new { isValid = true, message = _localization["Counties.Notification.Add.Text"].Value });
            }

            var cities = await _cityApiService.GetAllAsync();

            var countyAddViewModel = new CountyAddViewModel()
            {
                CityList = (cities)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   })
            };

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Add", countyAddViewModel) });
        }

        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(CountyUpdateRequestDto countyUpdateRequestDto)
        {
            if (ModelState.IsValid)
            {
                await _countyApiService.UpdateAsync(countyUpdateRequestDto);

                return Json(new { isValid = true, message = _localization["Counties.Notification.Edit.Text"].Value });
            }

            var cities = await _cityApiService.GetAllAsync();

            var countyUpdateViewModel = new CountyUpdateViewModel()
            {
                CityList = (cities)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   })
            };

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Edit", countyUpdateViewModel) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _countyApiService.RemoveAsync(id);

            return Json(new { message = _localization["Counties.Notification.Delete.Text"].Value });
        }
    }
}
