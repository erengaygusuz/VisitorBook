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
    public class SubRegionController : Controller
    {
        private readonly IStringLocalizer<Language> _localization;
        private readonly RazorViewConverter _razorViewConverter;
        private readonly RegionApiService _regionApiService;
        private readonly SubRegionApiService _subRegionApiService;
        private readonly SubRegionDataTablesOptions _subRegionDataTableOptions;

        public SubRegionController(RazorViewConverter razorViewConverter,
            IStringLocalizer<Language> localization, SubRegionApiService subRegionApiService, SubRegionDataTablesOptions subRegionDataTableOptions,
            RegionApiService regionApiService)
        {
            _subRegionApiService = subRegionApiService;
            _localization = localization;
            _razorViewConverter = razorViewConverter;
            _subRegionDataTableOptions = subRegionDataTableOptions;
            _regionApiService = regionApiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAll()
        {
            _subRegionDataTableOptions.SetDataTableOptions(Request);

            var result = await _subRegionApiService.GetTableData(_subRegionDataTableOptions.GetDataTablesOptions());

            return Json(new
            {
                recordsFiltered = result.RecordsFiltered,
                recordsTotal = result.RecordsTotal,
                data = result.Data
            });
        }

        public async Task<IActionResult> GetAllByRegion(int subRegionId)
        {
            var subregions = await _subRegionApiService.GetAllByRegionAsync(subRegionId);

            return Json(new
            {
                data = subregions
            });
        }

        [NoDirectAccess]
        public async Task<IActionResult> Add()
        {
            var regions = await _regionApiService.GetAllAsync();

            var subRegionViewModel = new SubRegionViewModel()
            {
                RegionList = (regions)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                SubRegion = new SubRegionInput()
            };

            return View(subRegionViewModel);
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id)
        {
            var subRegion = await _subRegionApiService.GetByIdAsync(id);

            var regions = await _regionApiService.GetAllAsync();

            var subRegionViewModel = new SubRegionViewModel()
            {
                RegionList = (regions)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                SubRegion = new SubRegionInput()
                {
                    Name = subRegion.Name,
                    RegionId = subRegion.Region.Id
                }
            };

            ViewData["Id"] = id;

            return View(subRegionViewModel);
        }

        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(SubRegionViewModel subRegionViewModel)
        {
            if (ModelState.IsValid)
            {
                await _subRegionApiService.AddAsync(subRegionViewModel.SubRegion);

                return Json(new { isValid = true, message = _localization["Counties.Notification.Add.Text"].Value });
            }

            var regions = await _regionApiService.GetAllAsync();

            subRegionViewModel.RegionList = (regions)
                  .Select(u => new SelectListItem
                  {
                      Text = u.Name,
                      Value = u.Id.ToString()
                  });

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Add", subRegionViewModel) });
        }

        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id, SubRegionViewModel subRegionViewModel)
        {
            if (ModelState.IsValid)
            {
                await _subRegionApiService.UpdateAsync(id, subRegionViewModel.SubRegion);

                return Json(new { isValid = true, message = _localization["Counties.Notification.Edit.Text"].Value });
            }

            var regions = await _regionApiService.GetAllAsync();

            subRegionViewModel.RegionList = (regions)
                  .Select(u => new SelectListItem
                  {
                      Text = u.Name,
                      Value = u.Id.ToString()
                  });

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Edit", subRegionViewModel) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _subRegionApiService.RemoveAsync(id);

            if (result)
            {
                return Json(new { message = _localization["Counties.Notification.SuccessfullDelete.Text"].Value });
            }

            return BadRequest(new { message = _localization["Counties.Notification.UnSuccessfullDelete.Text"].Value });
        }
    }
}
