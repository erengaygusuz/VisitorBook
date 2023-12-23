using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using VisitorBook.Frontend.UI.Attributes;
using VisitorBook.Frontend.UI.Configurations;
using VisitorBook.Frontend.UI.Languages;
using VisitorBook.Frontend.UI.Models.Inputs;
using VisitorBook.Frontend.UI.Services;
using VisitorBook.Frontend.UI.Utilities;
using VisitorBook.Frontend.UI.ViewModels;

namespace VisitorBook.Frontend.UI.Areas.Admin.Controllers
{
    [Area("App")]
    public class CountryController : Controller
    {
        private readonly IStringLocalizer<Language> _localization;
        private readonly RazorViewConverter _razorViewConverter;
        private readonly SubRegionApiService _subRegionApiService;
        private readonly CountryApiService _countryApiService;
        private readonly CountryDataTablesOptions _countryDataTableOptions;

        public CountryController(RazorViewConverter razorViewConverter,
            IStringLocalizer<Language> localization, SubRegionApiService subRegionApiService, CountryDataTablesOptions countryDataTableOptions, CountryApiService countryApiService)
        {
            _subRegionApiService = subRegionApiService;
            _localization = localization;
            _razorViewConverter = razorViewConverter;
            _countryDataTableOptions = countryDataTableOptions;
            _countryApiService = countryApiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAll()
        {
            _countryDataTableOptions.SetDataTableOptions(Request);

            var result = await _countryApiService.GetTableData(_countryDataTableOptions.GetDataTablesOptions());

            return Json(new
            {
                recordsFiltered = result.RecordsFiltered,
                recordsTotal = result.RecordsTotal,
                data = result.Data
            });
        }

        public async Task<IActionResult> GetAllBySubRegion(int subRegionId)
        {
            var countries = await _countryApiService.GetAllBySubRegionAsync(subRegionId);

            return Json(new
            {
                data = countries
            });
        }

        [NoDirectAccess]
        public async Task<IActionResult> Add()
        {
            var subRegions = await _subRegionApiService.GetAllAsync();

            var countryViewModel = new CountryViewModel()
            {
                SubRegionList = (subRegions)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                Country = new CountryInput()
            };

            return View(countryViewModel);
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id)
        {
            var country = await _countryApiService.GetByIdAsync(id);

            var subRegions = await _subRegionApiService.GetAllAsync();

            var countryViewModel = new CountryViewModel()
            {
                SubRegionList = (subRegions)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                Country = new CountryInput()
                {
                    Name = country.Name,
                    Code = country.Code,
                    SubRegionId = country.SubRegion.Id
                }
            };

            ViewData["Id"] = id;

            return View(countryViewModel);
        }

        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(CountryViewModel countryViewModel)
        {
            if (ModelState.IsValid)
            {
                await _countryApiService.AddAsync(countryViewModel.Country);

                return Json(new { isValid = true, message = _localization["Countries.Notification.Add.Text"].Value });
            }

            var subRegions = await _subRegionApiService.GetAllAsync();

            countryViewModel.SubRegionList = (subRegions)
                  .Select(u => new SelectListItem
                  {
                      Text = u.Name,
                      Value = u.Id.ToString()
                  });

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Add", countryViewModel) });
        }

        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id, CountryViewModel countryViewModel)
        {
            if (ModelState.IsValid)
            {
                await _countryApiService.UpdateAsync(id, countryViewModel.Country);

                return Json(new { isValid = true, message = _localization["Countries.Notification.Edit.Text"].Value });
            }

            var subRegions = await _subRegionApiService.GetAllAsync();

            countryViewModel.SubRegionList = (subRegions)
                  .Select(u => new SelectListItem
                  {
                      Text = u.Name,
                      Value = u.Id.ToString()
                  });

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Edit", countryViewModel) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _countryApiService.RemoveAsync(id);

            if (result)
            {
                return Json(new { message = _localization["Countries.Notification.SuccessfullDelete.Text"].Value });
            }

            return BadRequest(new { message = _localization["Countries.Notification.UnSuccessfullDelete.Text"].Value });
        }
    }
}
