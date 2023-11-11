using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Dtos.CityDtos;
using VisitorBook.Core.Dtos.CountyDtos;
using VisitorBook.Core.Dtos.VisitedCountyDtos;
using VisitorBook.Core.Dtos.VisitorDtos;
using VisitorBook.Core.Models;
using VisitorBook.Core.Utilities;
using VisitorBook.UI.Languages;
using VisitorBook.UI.ViewModels;

namespace VisitorBook.UI.Controllers
{
    public class VisitedCountyController : Controller
    {
        private readonly IService<County> _countyService;
        private readonly IService<Visitor> _visitorService;
        private readonly IService<VisitedCounty> _visitedCountyService;
        private readonly IService<City> _cityService;
        private readonly IStringLocalizer<Language> _localization;
        private readonly RazorViewConverter _razorViewConverter;
        private readonly IMapper _mapper;

        public VisitedCountyController(IService<County> countyService, IService<Visitor> visitorService,
                                       IService<VisitedCounty> visitedCountyService, IService<City> cityService, 
                                       IStringLocalizer<Language> localization, RazorViewConverter razorViewConverter,
                                       IMapper mapper)
        {
            _countyService = countyService;
            _visitorService = visitorService;
            _visitedCountyService = visitedCountyService;
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

            var visitedCounties = await _visitedCountyService.GetAllAsync(
                    page: page, pageSize: pageSize,
                    expression: (!string.IsNullOrEmpty(searchValue)) ?
                        (vc =>
                            (vc.Visitor.Name + " " + vc.Visitor.Surname).ToLower().Contains(searchValue.ToLower()) ||
                            vc.County.Name.ToLower().Contains(searchValue.ToLower()) ||
                            vc.County.City.Name.ToLower().Contains(searchValue.ToLower()))
                        : null,
                    include: u => u.Include(a => a.Visitor).Include(a => a.County).ThenInclude(b => b.City),
                    orderBy: (sortColumnDirection == "asc") ?
                        (o =>
                        o switch
                        {
                            _ when sortColumnIndex == "0" => o.OrderBy(s => s.Visitor.Name + " " + s.Visitor.Surname),
                            _ when sortColumnIndex == "1" => o.OrderBy(s => s.County.Name),
                            _ when sortColumnIndex == "2" => o.OrderBy(s => s.County.City.Name),
                            _ when sortColumnIndex == "3" => o.OrderBy(s => s.VisitDate),
                            _ => o.OrderBy(s => s.Visitor.Name + " " + s.Visitor.Surname)
                        })
                         :
                        (o =>
                        o switch
                        {
                            _ when sortColumnIndex == "0" => o.OrderByDescending(s => s.Visitor.Name + " " + s.Visitor.Surname),
                            _ when sortColumnIndex == "1" => o.OrderByDescending(s => s.County.Name),
                            _ when sortColumnIndex == "2" => o.OrderByDescending(s => s.County.City.Name),
                            _ when sortColumnIndex == "3" => o.OrderByDescending(s => s.VisitDate),
                            _ => o.OrderByDescending(s => s.Visitor.Name + " " + s.Visitor.Surname)
                        })
                );

            return Json(new
            {
                draw = draw,
                recordsFiltered = visitedCounties.Item2,
                recordsTotal = visitedCounties.Item1,
                data = visitedCounties.Item3
            });
        }

        public async Task<IActionResult> Add()
        {
            var cities = await _cityService.GetAllAsync(orderBy: o => o.OrderBy(x => x.Name));
            var cityGetResponseDtos = _mapper.Map<List<CityGetResponseDto>>(cities);

            var visitorsWithVisitorAddress = await _visitorService.GetAllAsync(v => v.VisitorAddressId != null);
            var visitorGetResponseDtos = _mapper.Map<List<VisitorGetResponseDto>>(visitorsWithVisitorAddress);

            var visitedCountyAddViewModel = new VisitedCountyAddViewModel()
            {
                VisitedCountyAddRequestDto = new VisitedCountyAddRequestDto(),
                CountyList = new List<SelectListItem>(),
                CityList = (cityGetResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                VisitorList = (visitorGetResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name + " " + u.Surname,
                       Value = u.Id.ToString()
                   })
            };

            return View(visitedCountyAddViewModel);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var visitedCounty = await _visitedCountyService.GetAsync(u => u.Id == id, include: a => a.Include(b => b.County));
            var visitedCountyUpdateRequestDto = _mapper.Map<VisitedCountyUpdateRequestDto>(visitedCounty);

            var counties = await _countyService.GetAllAsync(u => u.CityId == visitedCounty.County.CityId);
            var countyGetResponseDtos = _mapper.Map<List<CountyGetResponseDto>>(counties);

            var cities = await _cityService.GetAllAsync(orderBy: o => o.OrderBy(x => x.Name));
            var cityGetResponseDtos = _mapper.Map<List<CityGetResponseDto>>(cities);

            var visitorsWithVisitorAddress = await _visitorService.GetAllAsync(v => v.VisitorAddressId != null);
            var visitorGetResponseDtos = _mapper.Map<List<VisitorGetResponseDto>>(visitorsWithVisitorAddress);

            var visitedUpdateCountyViewModel = new VisitedCountyUpdateViewModel()
            {
                VisitedCountyUpdateRequestDto = visitedCountyUpdateRequestDto,
                CountyList = (countyGetResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                CityList = (cityGetResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                VisitorList = (visitorGetResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name + " " + u.Surname,
                       Value = u.Id.ToString()
                   })
            };

            return View(visitedUpdateCountyViewModel);
        }

        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(VisitedCountyAddRequestDto visitedCountyAddRequestDto)
        {
            if (ModelState.IsValid)
            {
                var visitedCounty = _mapper.Map<VisitedCounty>(visitedCountyAddRequestDto);

                await _visitedCountyService.AddAsync(visitedCounty);

                return Json(new { isValid = true, message = _localization["VisitedCounties.Notification.Add.Text"].Value });
            }

            var cities = await _cityService.GetAllAsync(orderBy: o => o.OrderBy(x => x.Name));
            var cityGetResponseDtos = _mapper.Map<List<CityGetResponseDto>>(cities);

            var visitorsWithVisitorAddress = await _visitorService.GetAllAsync(v => v.VisitorAddressId != null);
            var visitorGetResponseDtos = _mapper.Map<List<VisitorGetResponseDto>>(visitorsWithVisitorAddress);

            var visitedCountyAddViewModel = new VisitedCountyAddViewModel()
            {
                CityList = (cityGetResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                VisitorList = (visitorGetResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name + " " + u.Surname,
                       Value = u.Id.ToString()
                   })
            };

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Add", visitedCountyAddViewModel) });
        }

        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(VisitedCountyUpdateRequestDto visitedCountyUpdateRequestDto)
        {
            if (ModelState.IsValid)
            {
                var visitedCounty = await _visitedCountyService.GetAsync(u => u.Id == visitedCountyUpdateRequestDto.Id);

                visitedCounty.VisitDate = visitedCountyUpdateRequestDto.VisitDate;
                visitedCounty.VisitorId = visitedCountyUpdateRequestDto.VisitorId;

                //var newCounty = await _countyService.GetAsync(u => u.Id == visitedCountyUpdateRequest.CountyId);

                visitedCounty.CountyId = visitedCountyUpdateRequestDto.CountyId;

                await _visitedCountyService.UpdateAsync(visitedCounty);

                return Json(new { isValid = true, message = _localization["VisitedCounties.Notification.Edit.Text"].Value });
            }

            var cities = await _cityService.GetAllAsync(orderBy: o => o.OrderBy(x => x.Name));
            var cityGetResponseDtos = _mapper.Map<List<CityGetResponseDto>>(cities);

            var visitorsWithVisitorAddress = await _visitorService.GetAllAsync(v => v.VisitorAddressId != null);
            var visitorGetResponseDtos = _mapper.Map<List<VisitorGetResponseDto>>(visitorsWithVisitorAddress);

            var visitedCountyUpdateViewModel = new VisitedCountyUpdateViewModel()
            {
                CityList = (cityGetResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                VisitorList = (visitorGetResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name + " " + u.Surname,
                       Value = u.Id.ToString()
                   })
            };

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Edit", visitedCountyUpdateViewModel) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var visitedCounty = await _visitedCountyService.GetAsync(u => u.Id == id);

            var visitor = await _visitorService.GetAsync(u => u.Id == visitedCounty.VisitorId);
            var county = await _countyService.GetAsync(u => u.Id == visitedCounty.CountyId, include: u => u.Include(a => a.City));

            var visitedCountyInfo = "Visited County Record for " + visitor.Name + " " + visitor.Surname + " at " + county.Name + "/" + county.City.Name;

            if (visitedCounty != null)
            {
                await _visitedCountyService.RemoveAsync(visitedCounty);
            }

            return Json(new { message = _localization["VisitedCounties.Notification.Delete.Text"].Value });
        }
    }
}
