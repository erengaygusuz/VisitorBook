using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using VisitorBook.UI.Attributes;
using VisitorBook.UI.Configurations;
using VisitorBook.UI.Languages;
using Microsoft.AspNetCore.Mvc.Rendering;
using VisitorBook.UI.ViewModels;
using VisitorBook.Core.Utilities;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Entities;
using Microsoft.EntityFrameworkCore;
using VisitorBook.Core.Dtos.CityDtos;
using VisitorBook.UI.Areas.App.Controllers;
using VisitorBook.Core.Dtos.CountryDtos;
using Microsoft.AspNetCore.Authorization;

namespace VisitorBook.UI.Area.App.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("App")]
    public class CityController : BaseController
    {
        private readonly IStringLocalizer<Language> _localization;
        private readonly RazorViewConverter _razorViewConverter;
        private readonly IService<City> _cityService;
        private readonly IService<Country> _countryService;
        private readonly CityDataTablesOptions _cityDataTableOptions;

        public CityController(RazorViewConverter razorViewConverter, 
            IStringLocalizer<Language> localization, IService<City> cityService, CityDataTablesOptions cityDataTableOptions, 
            IService<Country> countryService)
        {
            _cityService = cityService;
            _localization = localization;
            _razorViewConverter = razorViewConverter;
            _cityDataTableOptions = cityDataTableOptions;
            _countryService = countryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetAll()
        {
            _cityDataTableOptions.SetDataTableOptions(Request);

            var result = _cityService.GetAll<CityResponseDto>(_cityDataTableOptions.GetDataTablesOptions(), include: x => x.Include(c => c.Country));

            return DataTablesResult(result);
        }

        public async Task<IActionResult> GetAllByCountry(int countryId)
        {
            var cityResponseDtos = await _cityService.GetAllAsync<CityResponseDto>(
                orderBy: o => o.OrderBy(x => x.Name),
                expression: u => u.CountryId == countryId,
                include: x => x.Include(c => c.Country));

            if (cityResponseDtos == null)
            {
                return NotFound();
            }

            return Json(new
            {
                data = cityResponseDtos
            });
        }

        [NoDirectAccess]
        public async Task<IActionResult> Add()
        {
            var countryResponseDtos = await _countryService.GetAllAsync<CountryResponseDto>();

            var cityViewModel = new CityViewModel()
            {
                CountryList = (countryResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                City = new CityRequestDto()
            };

            return View(cityViewModel);
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id)
        {
            var cityResponseDto = await _cityService.GetAsync<CityResponseDto>(x => x.Id == id, include: x => x.Include(c => c.Country));

            var countryResponseDtos = await _countryService.GetAllAsync<CountryResponseDto>();

            var cityViewModel = new CityViewModel()
            {
                CountryList = (countryResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                City = new CityRequestDto()
                {
                    Id = id,
                    Name = cityResponseDto.Name,
                    Code = cityResponseDto.Code,
                    CountryId = cityResponseDto.Country.Id
                }
            };

            return View(cityViewModel);
        }

        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(CityViewModel cityViewModel)
        {
            if (ModelState.IsValid)
            {
                await _cityService.AddAsync(cityViewModel.City);

                return Json(new { isValid = true, message = _localization["Cities.Notification.Add.Text"].Value });
            }

            var countryResponseDtos = await _countryService.GetAllAsync<CountryResponseDto>();

            cityViewModel.CountryList = (countryResponseDtos)
                  .Select(u => new SelectListItem
                  {
                      Text = u.Name,
                      Value = u.Id.ToString()
                  });

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Add", cityViewModel) });
        }

        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(CityViewModel cityViewModel)
        {
            if (ModelState.IsValid)
            {
                await _cityService.UpdateAsync(cityViewModel.City);

                return Json(new { isValid = true, message = _localization["Cities.Notification.Edit.Text"].Value });
            }

            var countryResponseDtos = await _countryService.GetAllAsync<CountryResponseDto>();

            cityViewModel.CountryList = (countryResponseDtos)
                  .Select(u => new SelectListItem
                  {
                      Text = u.Name,
                      Value = u.Id.ToString()
                  });

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Edit", cityViewModel) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var cityResponseDto = await _cityService.GetAsync<CityResponseDto>(u => u.Id == id);

            if (cityResponseDto == null)
            {
                return NotFound();
            }

            var result = await _cityService.RemoveAsync(cityResponseDto);

            if (result)
            {
                return Json(new { message = _localization["Cities.Notification.SuccessfullDelete.Text"].Value });
            }

            return BadRequest(new { message = _localization["Cities.Notification.UnSuccessfullDelete.Text"].Value });
        }
    }
}
