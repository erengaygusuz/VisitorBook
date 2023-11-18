using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using VisitorBook.UI.Attributes;
using VisitorBook.UI.Utilities;
using VisitorBook.UI.Configurations;
using VisitorBook.UI.Languages;
using VisitorBook.UI.Services;
using VisitorBook.UI.Models.Inputs;

namespace VisitorBook.UI.Controllers
{
    public class CityController : Controller
    {
        private readonly IStringLocalizer<Language> _localization;
        private readonly RazorViewConverter _razorViewConverter;
        private readonly CityApiService _cityApiService;
        private readonly CityDataTablesOptions _cityDataTableOptions;

        public CityController(RazorViewConverter razorViewConverter, 
            IStringLocalizer<Language> localization, CityApiService cityApiService, CityDataTablesOptions cityDataTableOptions)
        {
            _cityApiService = cityApiService;
            _localization = localization;
            _razorViewConverter = razorViewConverter;
            _cityDataTableOptions = cityDataTableOptions;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAll()
        {
            _cityDataTableOptions.SetDataTableOptions(Request);

            var result = await _cityApiService.GetTableData(_cityDataTableOptions.GetDataTablesOptions());

            return Json(new
            {
                recordsFiltered = result.RecordsFiltered,
                recordsTotal = result.RecordsTotal,
                data = result.Data
            });
        }

        [NoDirectAccess]
        public IActionResult Add()
        {
            return View();
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(Guid id)
        {
            var city = await _cityApiService.GetByIdAsync(id);

            var cityInput = new CityInput()
            {
                Name = city.Name,
                Code = city.Code
            };

            ViewData["Id"] = id;

            return View(cityInput);
        }

        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(CityInput city)
        {
            if (ModelState.IsValid)
            {
                await _cityApiService.AddAsync(city);

                return Json(new { isValid = true, message = _localization["Cities.Notification.Add.Text"].Value });
            }

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Add", city) });
        }

        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(Guid id, CityInput city)
        {
            if (ModelState.IsValid)
            {
                await _cityApiService.UpdateAsync(id, city);

                return Json(new { isValid = true, message = _localization["Cities.Notification.Edit.Text"].Value });
            }

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Edit", city) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
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
