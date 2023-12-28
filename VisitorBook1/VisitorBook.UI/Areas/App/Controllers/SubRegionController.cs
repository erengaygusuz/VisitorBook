using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using VisitorBook.UI.Attributes;
using VisitorBook.UI.Configurations;
using VisitorBook.UI.Languages;
using VisitorBook.Core.Utilities;
using VisitorBook.UI.ViewModels;
using VisitorBook.Core.Entities;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Dtos.CityDtos;
using VisitorBook.UI.Areas.App.Controllers;
using Microsoft.EntityFrameworkCore;
using VisitorBook.Core.Dtos.SubRegionDtos;
using VisitorBook.Core.Dtos.RegionDtos;

namespace VisitorBook.UI.Areas.Admin.Controllers
{
    [Area("App")]
    public class SubRegionController : BaseController
    {
        private readonly IStringLocalizer<Language> _localization;
        private readonly RazorViewConverter _razorViewConverter;
        private readonly IService<Region> _regionService;
        private readonly IService<SubRegion> _subRegionService;
        private readonly SubRegionDataTablesOptions _subRegionDataTableOptions;

        public SubRegionController(RazorViewConverter razorViewConverter,
            IStringLocalizer<Language> localization, IService<SubRegion> subRegionService, SubRegionDataTablesOptions subRegionDataTableOptions,
            IService<Region> regionService)
        {
            _subRegionService = subRegionService;
            _localization = localization;
            _razorViewConverter = razorViewConverter;
            _subRegionDataTableOptions = subRegionDataTableOptions;
            _regionService = regionService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetAll()
        {
            _subRegionDataTableOptions.SetDataTableOptions(Request);

            var result = _subRegionService.GetAll<SubRegionResponseDto>(_subRegionDataTableOptions.GetDataTablesOptions(), include: x => x.Include(c => c.Region));

            return DataTablesResult(result);
        }

        public async Task<IActionResult> GetAllByRegion(int regionId)
        {
            var subRegionResponseDtos = await _subRegionService.GetAllAsync<SubRegionResponseDto>(
                orderBy: o => o.OrderBy(x => x.Name),
                expression: u => u.RegionId == regionId,
                include: x => x.Include(c => c.Region));

            if (subRegionResponseDtos == null)
            {
                return NotFound();
            }

            return Json(new
            {
                data = subRegionResponseDtos
            });
        }

        [NoDirectAccess]
        public async Task<IActionResult> Add()
        {
            var regionResponseDtos = await _regionService.GetAllAsync<RegionResponseDto>();

            var subRegionViewModel = new SubRegionViewModel()
            {
                RegionList = (regionResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                SubRegion = new SubRegionRequestDto()
            };

            return View(subRegionViewModel);
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id)
        {
            var subRegionResponseDto = await _subRegionService.GetAsync<SubRegionResponseDto>(x => x.Id == id);

            var regionResponseDtos = await _regionService.GetAllAsync<RegionResponseDto>();

            var subRegionViewModel = new SubRegionViewModel()
            {
                RegionList = (regionResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                SubRegion = new SubRegionRequestDto()
                {
                    Name = subRegionResponseDto.Name,
                    RegionId = subRegionResponseDto.Region.Id
                }
            };

            return View(subRegionViewModel);
        }

        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(SubRegionViewModel subRegionViewModel)
        {
            if (ModelState.IsValid)
            {
                await _subRegionService.AddAsync(subRegionViewModel.SubRegion);

                return Json(new { isValid = true, message = _localization["SubRegions.Notification.Add.Text"].Value });
            }

            var regionResponseDtos = await _regionService.GetAllAsync<RegionResponseDto>();

            subRegionViewModel.RegionList = (regionResponseDtos)
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
        public async Task<IActionResult> EditPost(SubRegionViewModel subRegionViewModel)
        {
            if (ModelState.IsValid)
            {
                await _subRegionService.UpdateAsync(subRegionViewModel.SubRegion);

                return Json(new { isValid = true, message = _localization["SubRegions.Notification.Edit.Text"].Value });
            }

            var regionResponseDtos = await _regionService.GetAllAsync<RegionResponseDto>();

            subRegionViewModel.RegionList = (regionResponseDtos)
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
            var subRegionResponseDto = await _subRegionService.GetAsync<SubRegionResponseDto>(u => u.Id == id);

            if (subRegionResponseDto == null)
            {
                return NotFound();
            }

            var result = await _subRegionService.RemoveAsync(subRegionResponseDto);

            if (result)
            {
                return Json(new { message = _localization["SubRegions.Notification.SuccessfullDelete.Text"].Value });
            }

            return BadRequest(new { message = _localization["SubRegions.Notification.UnSuccessfullDelete.Text"].Value });
        }
    }
}
