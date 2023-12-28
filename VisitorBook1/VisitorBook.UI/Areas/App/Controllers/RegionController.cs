using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using VisitorBook.UI.Attributes;
using VisitorBook.UI.Configurations;
using VisitorBook.UI.Languages;
using VisitorBook.Core.Utilities;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Entities;
using VisitorBook.Core.Dtos.RegionDtos;
using VisitorBook.UI.Areas.App.Controllers;

namespace VisitorBook.UI.Areas.Admin.Controllers
{
    [Area("App")]
    public class RegionController : BaseController
    {
        private readonly IStringLocalizer<Language> _localization;
        private readonly RazorViewConverter _razorViewConverter;
        private readonly IService<Region> _regionService;
        private readonly RegionDataTablesOptions _regionDataTableOptions;

        public RegionController(RazorViewConverter razorViewConverter,
            IStringLocalizer<Language> localization, IService<Region> regionService, RegionDataTablesOptions regionDataTableOptions)
        {
            _regionService = regionService;
            _localization = localization;
            _razorViewConverter = razorViewConverter;
            _regionDataTableOptions = regionDataTableOptions;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetAll()
        {
            _regionDataTableOptions.SetDataTableOptions(Request);

            var result = _regionService.GetAll<RegionResponseDto>(_regionDataTableOptions.GetDataTablesOptions());

            return DataTablesResult(result);
        }

        [NoDirectAccess]
        public IActionResult Add()
        {
            return View();
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id)
        {
            var regionResponseDto = await _regionService.GetAsync<RegionResponseDto>(x => x.Id == id);

            var regionRequestDto = new RegionRequestDto()
            {
                Id = id,
                Name = regionResponseDto.Name
            };

            return View(regionRequestDto);
        }

        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(RegionRequestDto regionRequestDto)
        {
            if (ModelState.IsValid)
            {
                await _regionService.AddAsync(regionRequestDto);

                return Json(new { isValid = true, message = _localization["Regions.Notification.Add.Text"].Value });
            }

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Add", regionRequestDto) });
        }

        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(RegionRequestDto regionRequestDto)
        {
            if (ModelState.IsValid)
            {
                await _regionService.UpdateAsync(regionRequestDto);

                return Json(new { isValid = true, message = _localization["Regions.Notification.Edit.Text"].Value });
            }

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Edit", regionRequestDto) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var regionResponseDto = await _regionService.GetAsync<RegionResponseDto>(u => u.Id == id);

            if (regionResponseDto == null)
            {
                return NotFound();
            }

            var result = await _regionService.RemoveAsync(regionResponseDto);

            if (result)
            {
                return Json(new { message = _localization["Regions.Notification.SuccessfullDelete.Text"].Value });
            }

            return BadRequest(new { message = _localization["Regions.Notification.UnSuccessfullDelete.Text"].Value });
        }
    }
}
