using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using VisitorBook.Frontend.UI.Attributes;
using VisitorBook.Frontend.UI.Configurations;
using VisitorBook.Frontend.UI.Languages;
using VisitorBook.Frontend.UI.Models.Inputs;
using VisitorBook.Frontend.UI.Services;
using VisitorBook.Frontend.UI.Utilities;

namespace VisitorBook.Frontend.UI.Areas.Admin.Controllers
{
    [Area("App")]
    public class RegionController : Controller
    {
        private readonly IStringLocalizer<Language> _localization;
        private readonly RazorViewConverter _razorViewConverter;
        private readonly RegionApiService _regionApiService;
        private readonly RegionDataTablesOptions _regionDataTableOptions;

        public RegionController(RazorViewConverter razorViewConverter,
            IStringLocalizer<Language> localization, RegionApiService regionApiService, RegionDataTablesOptions regionDataTableOptions)
        {
            _regionApiService = regionApiService;
            _localization = localization;
            _razorViewConverter = razorViewConverter;
            _regionDataTableOptions = regionDataTableOptions;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAll()
        {
            _regionDataTableOptions.SetDataTableOptions(Request);

            var result = await _regionApiService.GetTableData(_regionDataTableOptions.GetDataTablesOptions());

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
        public async Task<IActionResult> Edit(int id)
        {
            var region = await _regionApiService.GetByIdAsync(id);

            var regionInput = new RegionInput()
            {
                Name = region.Name
            };

            ViewData["Id"] = id;

            return View(regionInput);
        }

        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(RegionInput region)
        {
            if (ModelState.IsValid)
            {
                await _regionApiService.AddAsync(region);

                return Json(new { isValid = true, message = _localization["Regions.Notification.Add.Text"].Value });
            }

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Add", region) });
        }

        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id, RegionInput region)
        {
            if (ModelState.IsValid)
            {
                await _regionApiService.UpdateAsync(id, region);

                return Json(new { isValid = true, message = _localization["Regions.Notification.Edit.Text"].Value });
            }

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Edit", region) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _regionApiService.RemoveAsync(id);

            if (result)
            {
                return Json(new { message = _localization["Regions.Notification.SuccessfullDelete.Text"].Value });
            }

            return BadRequest(new { message = _localization["Regions.Notification.UnSuccessfullDelete.Text"].Value });
        }
    }
}
