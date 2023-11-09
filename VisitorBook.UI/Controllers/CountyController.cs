using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Dtos.CityDtos;
using VisitorBook.Core.Dtos.CountyDtos;
using VisitorBook.Core.Models;
using VisitorBook.Core.Utilities;
using VisitorBook.UI.Languages;
using VisitorBook.UI.ViewModels;

namespace VisitorBook.UI.Controllers
{
    public class CountyController : Controller
    {
        private readonly IService<County> _countyService;
        private readonly IService<City> _cityService;
        private readonly IStringLocalizer<Language> _localization;
        private readonly RazorViewConverter _razorViewConverter;
        private readonly IMapper _mapper;

        public CountyController(IService<County> countyService, IService<City> cityService, IStringLocalizer<Language> localization, 
            RazorViewConverter razorViewConverter, IMapper mapper)
        {
            _countyService = countyService;
            _cityService = cityService;
            _localization = localization;
            _razorViewConverter = razorViewConverter;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAll()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumnIndex = Request.Form["order[0][column]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int page = (skip / pageSize) + 1;

            var counties = await _countyService.GetAllAsync(
                    page: page, pageSize: pageSize,
                    expression: (!string.IsNullOrEmpty(searchValue)) ?
                        (c =>
                            c.Name.ToLower().Contains(searchValue.ToLower()) ||
                            c.City.Name.ToLower().Contains(searchValue.ToLower()) ||
                            c.Latitude.ToString().ToLower().Contains(searchValue.ToLower()) ||
                            c.Longitude.ToString().ToLower().Contains(searchValue.ToLower()))
                        : null,
                    include: u => u.Include(a => a.City),
                    orderBy: (sortColumnDirection == "asc") ?
                        (o =>
                        o switch
                        {
                            _ when sortColumnIndex == "0" => o.OrderBy(s => s.Name),
                            _ when sortColumnIndex == "1" => o.OrderBy(s => s.City.Name),
                            _ when sortColumnIndex == "2" => o.OrderBy(s => s.Latitude),
                            _ when sortColumnIndex == "3" => o.OrderBy(s => s.Longitude),
                            _ => o.OrderBy(s => s.Name)
                        })
                         :
                        (o =>
                        o switch
                        {
                            _ when sortColumnIndex == "0" => o.OrderByDescending(s => s.Name),
                            _ when sortColumnIndex == "1" => o.OrderByDescending(s => s.City.Name),
                            _ when sortColumnIndex == "2" => o.OrderByDescending(s => s.Latitude),
                            _ when sortColumnIndex == "3" => o.OrderByDescending(s => s.Longitude),
                            _ => o.OrderByDescending(s => s.Name)
                        })
                );

            return Json(new
            {
                draw = draw,
                recordsFiltered = counties.Item2,
                recordsTotal = counties.Item1,
                data = counties.Item3
            });
        }

        public async Task<IActionResult> GetAllByCity(Guid cityId)
        {
            var counties = await _countyService.GetAllAsync(u => u.CityId == cityId);

            return Json(new
            {
                data = counties
            });
        }

        public async Task<IActionResult> Add()
        {
            var cities = await _cityService.GetAllAsync();
            var cityGetResponseDtos = _mapper.Map<List<CityGetResponseDto>>(cities);

            var countyAddViewModel = new CountyAddViewModel()
            {
                CityList = (cityGetResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                CountyAddRequestDto = new CountyAddRequestDto()
            };

            return View(countyAddViewModel);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var county = await _countyService.GetAsync(u => u.Id == id);
            var countyUpdateRequestDto = _mapper.Map<CountyUpdateRequestDto>(county);

            var cities = await _cityService.GetAllAsync();
            var cityGetResponseDtos = _mapper.Map<List<CityGetResponseDto>>(cities);

            var countyUpdateViewModel = new CountyUpdateViewModel()
            {
                CityList = (cityGetResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                CountyUpdateRequestDto = countyUpdateRequestDto
            };

            return View(countyUpdateViewModel);
        }

        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(CountyAddRequestDto countyAddRequestDto)
        {
            if (ModelState.IsValid)
            {
                var county = _mapper.Map<County>(countyAddRequestDto);

                await _countyService.AddAsync(county);

                return Json(new { isValid = true, message = _localization["Counties.Notification.Add.Text"].Value });
            }

            var cities = await _cityService.GetAllAsync();
            var cityGetResponseDtos = _mapper.Map<List<CityGetResponseDto>>(cities);

            var countyAddViewModel = new CountyAddViewModel()
            {
                CityList = (cityGetResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   })
            };

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Add", countyAddViewModel) });
        }

        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(CountyUpdateRequestDto countyUpdateRequestDto)
        {
            if (ModelState.IsValid)
            {
                var county = _mapper.Map<County>(countyUpdateRequestDto);

                await _countyService.UpdateAsync(county);

                return Json(new { isValid = true, message = _localization["Counties.Notification.Edit.Text"].Value });
            }

            var cities = await _cityService.GetAllAsync();
            var cityGetResponseDtos = _mapper.Map<List<CityGetResponseDto>>(cities);

            var countyUpdateViewModel = new CountyUpdateViewModel()
            {
                CityList = (cityGetResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   })
            };

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Edit", countyUpdateViewModel) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var county = await _countyService.GetAsync(u => u.Id == id);

            var countyName = county.Name;

            if (county != null)
            {
                await _countyService.RemoveAsync(county);
            }

            return Json(new { message = _localization["Counties.Notification.Delete.Text"].Value });
        }
    }
}
