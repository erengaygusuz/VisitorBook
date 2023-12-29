﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Dtos.CountryDtos;
using VisitorBook.Core.Dtos.SubRegionDtos;
using VisitorBook.Core.Entities;
using VisitorBook.Core.Utilities;
using VisitorBook.UI.Areas.App.Controllers;
using VisitorBook.UI.Attributes;
using VisitorBook.UI.Configurations;
using VisitorBook.UI.Languages;
using VisitorBook.UI.ViewModels;

namespace VisitorBook.UI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("App")]
    public class CountryController : BaseController
    {
        private readonly IStringLocalizer<Language> _localization;
        private readonly RazorViewConverter _razorViewConverter;
        private readonly IService<SubRegion> _subRegionService;
        private readonly IService<Country> _countryService;
        private readonly CountryDataTablesOptions _countryDataTableOptions;

        public CountryController(RazorViewConverter razorViewConverter,
            IStringLocalizer<Language> localization, IService<SubRegion> subRegionService, CountryDataTablesOptions countryDataTableOptions, IService<Country> countryService)
        {
            _subRegionService = subRegionService;
            _localization = localization;
            _razorViewConverter = razorViewConverter;
            _countryDataTableOptions = countryDataTableOptions;
            _countryService = countryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetAll()
        {
            _countryDataTableOptions.SetDataTableOptions(Request);

            var result = _countryService.GetAll<CountryResponseDto>(_countryDataTableOptions.GetDataTablesOptions(), include: x => x.Include(c => c.SubRegion));

            return DataTablesResult(result);
        }

        public async Task<IActionResult> GetAllBySubRegion(int subRegionId)
        {
            var countryResponseDtos = await _countryService.GetAllAsync<CountryResponseDto>(
                orderBy: o => o.OrderBy(x => x.Name),
                expression: u => u.SubRegionId == subRegionId,
                include: x => x.Include(c => c.SubRegion));

            if (countryResponseDtos == null)
            {
                return NotFound();
            }

            return Json(new
            {
                data = countryResponseDtos
            });
        }

        [NoDirectAccess]
        public async Task<IActionResult> Add()
        {
            var subRegionResponseDtos = await _subRegionService.GetAllAsync<SubRegionResponseDto>();

            var countryViewModel = new CountryViewModel()
            {
                SubRegionList = (subRegionResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                Country = new CountryRequestDto()
            };

            return View(countryViewModel);
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id)
        {
            var countryResponseDto = await _countryService.GetAsync<CountryResponseDto>(x => x.Id == id, include: x => x.Include(c => c.SubRegion));

            var subRegionResponseDtos = await _subRegionService.GetAllAsync<SubRegionResponseDto>();

            var countryViewModel = new CountryViewModel()
            {
                SubRegionList = (subRegionResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                Country = new CountryRequestDto()
                {
                    Id = id,
                    Name = countryResponseDto.Name,
                    Code = countryResponseDto.Code,
                    SubRegionId = countryResponseDto.SubRegion.Id
                }
            };

            return View(countryViewModel);
        }

        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(CountryViewModel countryViewModel)
        {
            if (ModelState.IsValid)
            {
                await _countryService.AddAsync(countryViewModel.Country);

                return Json(new { isValid = true, message = _localization["Countries.Notification.Add.Text"].Value });
            }

            var subRegionResponseDtos = await _subRegionService.GetAllAsync<SubRegionResponseDto>();

            countryViewModel.SubRegionList = (subRegionResponseDtos)
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
        public async Task<IActionResult> EditPost(CountryViewModel countryViewModel)
        {
            if (ModelState.IsValid)
            {
                await _countryService.UpdateAsync(countryViewModel.Country);

                return Json(new { isValid = true, message = _localization["Countries.Notification.Edit.Text"].Value });
            }

            var subRegionResponseDtos = await _subRegionService.GetAllAsync<SubRegionResponseDto>();

            countryViewModel.SubRegionList = (subRegionResponseDtos)
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
            var countryResponseDto = await _countryService.GetAsync<CountryResponseDto>(u => u.Id == id);

            if (countryResponseDto == null)
            {
                return NotFound();
            }

            var result = await _countryService.RemoveAsync(countryResponseDto);

            if (result)
            {
                return Json(new { message = _localization["Countries.Notification.SuccessfullDelete.Text"].Value });
            }

            return BadRequest(new { message = _localization["Countries.Notification.UnSuccessfullDelete.Text"].Value });
        }
    }
}
