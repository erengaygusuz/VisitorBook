using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Dtos.CityDtos;
using VisitorBook.Core.Dtos.CountyDtos;
using VisitorBook.Core.Dtos.VisitorAddressDtos;
using VisitorBook.Core.Dtos.VisitorDtos;
using VisitorBook.Core.Enums;
using VisitorBook.Core.Models;
using VisitorBook.Core.Utilities;
using VisitorBook.UI.Languages;
using VisitorBook.UI.ViewModels;

namespace VisitorBook.UI.Controllers
{
    public class VisitorController : Controller
    {
        private readonly IService<County> _countyService;
        private readonly IService<City> _cityService;
        private readonly IService<Visitor> _visitorService;
        private readonly IService<VisitorAddress> _visitorAddressService;
        private readonly IStringLocalizer<Language> _localization;
        private readonly RazorViewConverter _razorViewConverter;
        private readonly IMapper _mapper;

        public VisitorController(IService<County> countyService, IService<City> cityService,
            IService<Visitor> visitorService, IStringLocalizer<Language> localization, 
            IService<VisitorAddress> visitorAddressService, RazorViewConverter razorViewConverter, IMapper mapper)
        {
            _countyService = countyService;
            _cityService = cityService;
            _visitorService = visitorService;
            _localization = localization;
            _visitorAddressService = visitorAddressService;
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

            var visitors = await _visitorService.GetAllAsync(
                    page: page, pageSize: pageSize,
                    expression: (!string.IsNullOrEmpty(searchValue)) ?
                        (v =>
                            v.Name.ToLower().Contains(searchValue.ToLower()) ||
                            v.Surname.ToLower().Contains(searchValue.ToLower()) ||
                            v.BirthDate.ToString().ToLower().Contains(searchValue.ToLower()))
                        : null,
                    orderBy: (sortColumnDirection == "asc") ?
                        (o =>
                        o switch
                        {
                            _ when sortColumnIndex == "0" => o.OrderBy(s => s.Name),
                            _ when sortColumnIndex == "1" => o.OrderBy(s => s.Surname),
                            _ when sortColumnIndex == "2" => o.OrderBy(s => s.BirthDate),
                            _ when sortColumnIndex == "3" => o.OrderBy(s => s.Gender),
                            _ => o.OrderBy(s => s.Name)
                        })
                         :
                        (o =>
                        o switch
                        {
                            _ when sortColumnIndex == "0" => o.OrderByDescending(s => s.Name),
                            _ when sortColumnIndex == "1" => o.OrderByDescending(s => s.Surname),
                            _ when sortColumnIndex == "2" => o.OrderByDescending(s => s.BirthDate),
                            _ when sortColumnIndex == "3" => o.OrderByDescending(s => s.Gender),
                            _ => o.OrderByDescending(s => s.Name)
                        })
                );

            return Json(new
            {
                draw = draw,
                recordsFiltered = visitors.Item2,
                recordsTotal = visitors.Item1,
                data = visitors.Item3.Select(u => new
                {
                    Id = u.Id,
                    Name = u.Name,
                    Surname = u.Surname,
                    Gender = _localization["Enum.Gender." + u.Gender.ToString() + ".Text"].Value,
                    BirthDate = u.BirthDate
                })
            });
        }

        public async Task<IActionResult> Add()
        {
            var cities = await _cityService.GetAllAsync(orderBy: o => o.OrderBy(x => x.Name));
            var cityGetResponseDtos = _mapper.Map<List<CityGetResponseDto>>(cities);

            var visitorAddViewModel = new VisitorAddViewModel()
            {
                VisitorAddRequestDto = new VisitorAddRequestDto(),
                GenderList = new List<Gender> { Gender.Male, Gender.Female }
                    .Select(u => new SelectListItem
                    {
                        Text = _localization["Enum.Gender." + u.ToString() + ".Text"].Value,
                        Value = u.ToString()
                    }),
                CityList = (cityGetResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                CountyList = new List<SelectListItem>()
            };

            return View(visitorAddViewModel);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var visitor = await _visitorService.GetAsync(u => u.Id == id, include: u => u.Include(a => a.VisitorAddress).ThenInclude(b => b.County));
            var visitorUpdateRequestDto = _mapper.Map<VisitorUpdateRequestDto>(visitor);

            List<CountyGetResponseDto> countyGetResponseDtos;

            if (visitor.VisitorAddress != null)
            {
                var counties = await _countyService.GetAllAsync(u => u.CityId == visitor.VisitorAddress.County.CityId);
                countyGetResponseDtos = _mapper.Map<List<CountyGetResponseDto>>(counties);
            }

            else
            {
                countyGetResponseDtos = new List<CountyGetResponseDto>();
            }

            var cities = await _cityService.GetAllAsync();
            var cityGetResponseDtos = _mapper.Map<List<CityGetResponseDto>>(cities);

            var visitorUpdateViewModel = new VisitorUpdateViewModel()
            {
                VisitorUpdateRequestDto = visitorUpdateRequestDto,
                GenderList = new List<Gender> { Gender.Male, Gender.Female }
                    .Select(u => new SelectListItem
                    {
                        Text = _localization["Enum.Gender." + u.ToString() + ".Text"].Value,
                        Value = u.ToString()
                    }),
                CityList = (cityGetResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                CountyList = (countyGetResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   })
            };

            return View(visitorUpdateViewModel);
        }

        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(VisitorAddRequestDto visitorAddRequestDto)
        {
            if (ModelState.IsValid)
            {
                var visitor = _mapper.Map<Visitor>(visitorAddRequestDto);

                await _visitorService.AddAsync(visitor);

                return Json(new { isValid = true, message = _localization["Visitors.Notification.Add.Text"].Value });
            }

            var cities = await _cityService.GetAllAsync();
            var cityGetResponseDtos = _mapper.Map<List<CityGetResponseDto>>(cities);

            var visitorViewModel = new VisitorAddViewModel()
            {
                CityList = (cityGetResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                GenderList = new List<Gender> { Gender.Male, Gender.Female }
                   .Select(u => new SelectListItem
                   {
                       Text = _localization["Enum.Gender." + u.ToString() + ".Text"].Value,
                       Value = u.ToString()
                   })
            };

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Add", visitorViewModel) });
        }

        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(VisitorUpdateRequestDto visitorUpdateRequestDto)
        {
            if (ModelState.IsValid)
            {
                var visitor = _mapper.Map<Visitor>(visitorUpdateRequestDto);

                if (visitorUpdateRequestDto.VisitorAddressId != null)
                {
                    var visitorAddress = await _visitorAddressService.GetAsync(va => va.Id == visitorUpdateRequestDto.VisitorAddressId);

                    if (visitorAddress != null)
                    {
                        visitorAddress.CountyId = visitorUpdateRequestDto.CountyId;

                        await _visitorAddressService.UpdateAsync(visitorAddress);
                    }
                }

                else
                {
                    visitor.VisitorAddress = new VisitorAddress
                    {
                        CountyId = visitorUpdateRequestDto.CountyId
                    };
                }

                await _visitorService.UpdateAsync(visitor);

                return Json(new { isValid = true, message = _localization["Visitors.Notification.Edit.Text"].Value });
            }

            var visitorUpdateViewModel = new VisitorUpdateViewModel
            {
                CityList = (await _cityService.GetAllAsync())
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                GenderList = new List<Gender> { Gender.Male, Gender.Female }
                   .Select(u => new SelectListItem
                   {
                       Text = _localization["Enum.Gender." + u.ToString() + ".Text"].Value,
                       Value = u.ToString()
                   })
            };

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Edit", visitorUpdateViewModel) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var visitor = await _visitorService.GetAsync(u => u.Id == id, include: u => u.Include(a => a.VisitorAddress));

            var visitorName = visitor.Name + " " + visitor.Surname;

            if (visitor != null)
            {
                await _visitorService.RemoveAsync(visitor);

                if (visitor.VisitorAddress != null)
                {
                    await _visitorAddressService.RemoveAsync(visitor.VisitorAddress);
                }
            }

            return Json(new { message = _localization["Visitors.Notification.Delete.Text"].Value });
        }
    }
}
