using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Attributes;
using VisitorBook.Core.Dtos.CityDtos;
using VisitorBook.Core.Utilities;
using VisitorBook.UI.Configurations;
using VisitorBook.UI.Languages;
using VisitorBook.UI.Services;

namespace VisitorBook.UI.Controllers
{
    public class CityController : Controller
    {
        private readonly IStringLocalizer<Language> _localization;
        private readonly RazorViewConverter _razorViewConverter;
        private readonly CityApiService _cityApiService;
        private readonly CityDataTableOptions _cityDataTableOptions;

        public CityController(RazorViewConverter razorViewConverter, 
            IStringLocalizer<Language> localization, CityApiService cityApiService, CityDataTableOptions cityDataTableOptions)
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
            var city = await _cityApiService.GetByIdAsync<CityGetResponseDto>(id);

            return View(city);
        }

        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(CityAddRequestDto cityAddRequestDto)
        {
            if (ModelState.IsValid)
            {
                await _cityApiService.AddAsync(cityAddRequestDto);

                return Json(new { isValid = true, message = _localization["Cities.Notification.Add.Text"].Value });
            }

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Add", cityAddRequestDto) });
        }

        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(CityUpdateRequestDto cityUpdateRequestDto)
        {
            if (ModelState.IsValid)
            {
                await _cityApiService.UpdateAsync(cityUpdateRequestDto);

                return Json(new { isValid = true, message = _localization["Cities.Notification.Edit.Text"].Value });
            }

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Edit", cityUpdateRequestDto) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _cityApiService.RemoveAsync(id);

            return Json(new { message = _localization["Cities.Notification.Delete.Text"].Value });
        }
    }
}
