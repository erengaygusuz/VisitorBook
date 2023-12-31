using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using VisitorBook.UI.Attributes;
using VisitorBook.Core.Utilities;
using VisitorBook.UI.Configurations;
using VisitorBook.UI.Languages;
using VisitorBook.Core.ViewModels;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Entities;
using Microsoft.EntityFrameworkCore;
using VisitorBook.Core.Dtos.CountyDtos;
using VisitorBook.UI.Areas.App.Controllers;
using VisitorBook.Core.Dtos.CityDtos;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;
using VisitorBook.Core.Extensions;

namespace VisitorBook.UI.Area.App.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("App")]
    public class CountyController : BaseController
    {
        private readonly IStringLocalizer<Language> _localization;
        private readonly RazorViewConverter _razorViewConverter;
        private readonly IService<County> _countyService;
        private readonly IService<City> _cityService;
        private readonly CountyDataTablesOptions _countyDataTableOptions;
        private readonly IValidator<CountyViewModel> _countyViewModelValidator;

        public CountyController(IService<County> countyService, IService<City> cityService, IStringLocalizer<Language> localization, 
            RazorViewConverter razorViewConverter, CountyDataTablesOptions countyDataTableOptions, IValidator<CountyViewModel> countyViewModelValidator)
        {
            _countyService = countyService;
            _cityService = cityService;
            _localization = localization;
            _razorViewConverter = razorViewConverter;
            _countyDataTableOptions = countyDataTableOptions;
            _countyViewModelValidator = countyViewModelValidator;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetAll()
        {
            _countyDataTableOptions.SetDataTableOptions(Request);

            var result = _countyService.GetAll<CountyResponseDto>(_countyDataTableOptions.GetDataTablesOptions(), include: x => x.Include(c => c.City));

            return DataTablesResult(result);
        }

        public async Task<IActionResult> GetAllByCity(int cityId)
        {
            var countyResponseDtos = await _countyService.GetAllAsync<CountyResponseDto>(
                orderBy: o => o.OrderBy(x => x.Name),
                expression: u => u.CityId == cityId,
                include: x => x.Include(c => c.City));

            if (countyResponseDtos == null)
            {
                return NotFound();
            }

            return Json(new
            {
                data = countyResponseDtos
            });
        }

        [NoDirectAccess]
        public async Task<IActionResult> Add()
        {
            var cityResponseDtos = await _cityService.GetAllAsync<CityResponseDto>();

            var countyViewModel = new CountyViewModel()
            {
                CityList = (cityResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                County = new CountyRequestDto()
            };

            return View(countyViewModel);
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id)
        {
            var countyResponseDto = await _countyService.GetAsync<CountyResponseDto>(x => x.Id == id, include: x => x.Include(c => c.City));

            var cityResponseDtos = await _cityService.GetAllAsync<CityResponseDto>();

            var countyViewModel = new CountyViewModel()
            {
                CityList = (cityResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                County = new CountyRequestDto()
                {
                    Id = id,
                    Name = countyResponseDto.Name,
                    Latitude = countyResponseDto.Latitude,
                    Longitude = countyResponseDto.Longitude,
                    CityId = countyResponseDto.City.Id
                }
            };

            return View(countyViewModel);
        }

        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(CountyViewModel countyViewModel)
        {
            var validationResult = await _countyViewModelValidator.ValidateAsync(countyViewModel);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);

                var cityResponseDtos = await _cityService.GetAllAsync<CityResponseDto>();

                countyViewModel.CityList = (cityResponseDtos)
                      .Select(u => new SelectListItem
                      {
                          Text = u.Name,
                          Value = u.Id.ToString()
                      });

                return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Add", countyViewModel) });
            }

            await _countyService.AddAsync(countyViewModel.County);

            return Json(new { isValid = true, message = _localization["Counties.Notification.Add.Text"].Value });
        }

        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(CountyViewModel countyViewModel)
        {
            var validationResult = await _countyViewModelValidator.ValidateAsync(countyViewModel);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);

                var cityResponseDtos = await _cityService.GetAllAsync<CityResponseDto>();

                countyViewModel.CityList = (cityResponseDtos)
                       .Select(u => new SelectListItem
                       {
                           Text = u.Name,
                           Value = u.Id.ToString()
                       });

                return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Edit", countyViewModel) });
            }
            
            await _countyService.UpdateAsync(countyViewModel.County);

            return Json(new { isValid = true, message = _localization["Counties.Notification.Edit.Text"].Value });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var countyResponseDto = await _countyService.GetAsync<CountyResponseDto>(u => u.Id == id);

            if (countyResponseDto == null)
            {
                return NotFound();
            }

            var result = await _countyService.RemoveAsync(countyResponseDto);

            if (result)
            {
                return Json(new { message = _localization["Counties.Notification.SuccessfullDelete.Text"].Value });
            }

            return BadRequest(new { message = _localization["Counties.Notification.UnSuccessfullDelete.Text"].Value });
        }
    }
}
