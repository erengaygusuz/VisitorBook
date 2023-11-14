
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Dtos.CountyDtos;
using VisitorBook.Core.Dtos.VisitorAddressDtos;
using VisitorBook.Core.Dtos.VisitorDtos;
using VisitorBook.Core.Enums;
using VisitorBook.Core.Utilities;
using VisitorBook.UI.Configurations;
using VisitorBook.UI.Languages;
using VisitorBook.UI.Services;
using VisitorBook.UI.ViewModels;

namespace VisitorBook.UI.Controllers
{
    public class VisitorController : Controller
    {
        private readonly CountyApiService _countyApiService;
        private readonly CityApiService _cityApiService;
        private readonly VisitorApiService _visitorApiService;
        private readonly VisitorAddressApiService _visitorAddressApiService;
        private readonly IStringLocalizer<Language> _localization;
        private readonly RazorViewConverter _razorViewConverter;
        private readonly VisitorDataTablesOptions _visitorDataTableOptions;

        public VisitorController(CountyApiService countyApiService, CityApiService cityApiService,
            VisitorApiService visitorApiService, IStringLocalizer<Language> localization,
            VisitorAddressApiService visitorAddressApiService, RazorViewConverter razorViewConverter, 
            VisitorDataTablesOptions visitorDataTableOptions)
        {
            _countyApiService = countyApiService;
            _cityApiService = cityApiService;
            _visitorApiService = visitorApiService;
            _localization = localization;
            _visitorAddressApiService = visitorAddressApiService;
            _razorViewConverter = razorViewConverter;
            _visitorDataTableOptions = visitorDataTableOptions;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAll()
        {
            _visitorDataTableOptions.SetDataTableOptions(Request);
            var result = await _visitorApiService.GetTableData(_visitorDataTableOptions.GetDataTablesOptions());

            return Json(new
            {
                recordsFiltered = result.RecordsFiltered,
                recordsTotal = result.RecordsTotal,
                data = result.Data
            });
        }

        public async Task<IActionResult> Add()
        {
            var cities = await _cityApiService.GetAllAsync();

            var visitorAddViewModel = new VisitorAddViewModel()
            {
                VisitorAddRequestDto = new VisitorAddRequestDto(),
                GenderList = new List<Gender> { Gender.Male, Gender.Female }
                    .Select(u => new SelectListItem
                    {
                        Text = _localization["Enum.Gender." + u.ToString() + ".Text"].Value,
                        Value = u.ToString()
                    }),
                CityList = (cities)
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
            var visitor = await _visitorApiService.GetByIdAsync<VisitorUpdateRequestDto>(id);

            List<CountyGetResponseDto> counties;

            if (visitor.VisitorAddressId != null)
            {
                counties = await _countyApiService.GetAllByCityAsync(visitor.CityId);
            }

            else
            {
                counties = new List<CountyGetResponseDto>();
            }

            var cities = await _cityApiService.GetAllAsync();

            var visitorUpdateViewModel = new VisitorUpdateViewModel()
            {
                VisitorUpdateRequestDto = visitor,
                GenderList = new List<Gender> { Gender.Male, Gender.Female }
                    .Select(u => new SelectListItem
                    {
                        Text = _localization["Enum.Gender." + u.ToString() + ".Text"].Value,
                        Value = u.ToString()
                    }),
                CityList = (cities)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                CountyList = (counties)
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
                await _visitorApiService.AddAsync(visitorAddRequestDto);

                return Json(new { isValid = true, message = _localization["Visitors.Notification.Add.Text"].Value });
            }

            var cities = await _cityApiService.GetAllAsync();

            var visitorViewModel = new VisitorAddViewModel()
            {
                CityList = (cities)
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
                var visitor = await _visitorApiService.GetByIdAsync<VisitorUpdateRequestDto>(visitorUpdateRequestDto.Id);

                if (visitor.VisitorAddressId != null)
                {
                    var visitorAddress = await _visitorAddressApiService.GetByIdAsync<VisitorAddressUpdateRequestDto>(visitor.VisitorAddressId.Value);

                    if (visitorAddress != null)
                    {
                        visitorAddress.CountyId = visitorUpdateRequestDto.CountyId.Value;

                        await _visitorAddressApiService.UpdateAsync(visitorAddress);
                    }
                }

                else
                {
                    if (visitorUpdateRequestDto.CountyId != null)
                    {
                        await _visitorAddressApiService.AddAsync(new VisitorAddressAddRequestDto
                        {
                            CountyId = visitorUpdateRequestDto.CountyId.Value
                        });
                    }
                }

                await _visitorApiService.UpdateAsync(visitor);

                return Json(new { isValid = true, message = _localization["Visitors.Notification.Edit.Text"].Value });
            }

            var cities = await _cityApiService.GetAllAsync();

            var visitorUpdateViewModel = new VisitorUpdateViewModel
            {
                CityList = (cities)
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
            var result = await _visitorApiService.RemoveAsync(id);

            if (result)
            {
                return Json(new { message = _localization["Visitors.Notification.SuccessfullDelete.Text"].Value });
            }

            return BadRequest(new { message = _localization["Visitors.Notification.UnSuccessfullDelete.Text"].Value });
        }
    }
}
