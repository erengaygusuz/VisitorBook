using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using VisitorBook.UI.Configurations;
using VisitorBook.UI.Languages;
using VisitorBook.Core.Utilities;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Entities;
using VisitorBook.Core.Dtos.RegionDtos;
using VisitorBook.UI.Areas.App.Controllers;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;
using VisitorBook.Core.Extensions;
using VisitorBook.Core.Constants;

namespace VisitorBook.UI.Areas.AppControllers
{
    [Authorize]
    [Area("App")]
    public class RegionController : BaseController
    {
        private readonly IStringLocalizer<Language> _localization;
        private readonly RazorViewConverter _razorViewConverter;
        private readonly IService<Region> _regionService;
        private readonly RegionDataTablesOptions _regionDataTableOptions;
        private readonly IValidator<RegionRequestDto> _regionRequestDtoValidator;

        public RegionController(RazorViewConverter razorViewConverter,
            IStringLocalizer<Language> localization, IService<Region> regionService, RegionDataTablesOptions regionDataTableOptions, IValidator<RegionRequestDto> regionRequestDtoValidator)
        {
            _regionService = regionService;
            _localization = localization;
            _razorViewConverter = razorViewConverter;
            _regionDataTableOptions = regionDataTableOptions;
            _regionRequestDtoValidator = regionRequestDtoValidator;
        }

        [Authorize(Permissions.PlaceManagement.View)]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Permissions.PlaceManagement.View)]
        [HttpPost]
        public IActionResult GetAll()
        {
            _regionDataTableOptions.SetDataTableOptions(Request);

            var result = _regionService.GetAll<RegionResponseDto>(_regionDataTableOptions.GetDataTablesOptions());

            return DataTablesResult(result);
        }

        [Authorize(Permissions.PlaceManagement.Create)]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize(Permissions.PlaceManagement.Edit)]
        [HttpGet]
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

        [Authorize(Permissions.PlaceManagement.Create)]
        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(RegionRequestDto regionRequestDto)
        {
            var validationResult = await _regionRequestDtoValidator.ValidateAsync(regionRequestDto);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);

                return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Add", regionRequestDto) });
            }
            
            await _regionService.AddAsync(regionRequestDto);

            return Json(new { isValid = true, message = _localization["Regions.Notification.Add.Text"].Value });
        }

        [Authorize(Permissions.PlaceManagement.Edit)]
        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(RegionRequestDto regionRequestDto)
        {
            var validationResult = await _regionRequestDtoValidator.ValidateAsync(regionRequestDto);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);

                return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Edit", regionRequestDto) });
            }
            
            await _regionService.UpdateAsync(regionRequestDto);

            return Json(new { isValid = true, message = _localization["Regions.Notification.Edit.Text"].Value });
        }

        [Authorize(Permissions.PlaceManagement.Delete)]
        [HttpGet]
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

            return Json(new { message = _localization["Regions.Notification.UnSuccessfullDelete.Text"].Value });
        }
    }
}
