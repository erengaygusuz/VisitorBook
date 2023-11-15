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
                VisitorRequestDto = new VisitorRequestDto(),
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
            var visitor = await _visitorApiService.GetByIdAsync<VisitorResponseDto>(id);

            List<CountyResponseDto> counties;

            if (visitor.VisitorAddressId != null)
            {
                counties = await _countyApiService.GetAllByCityAsync(visitor.CityId);
            }

            else
            {
                counties = new List<CountyResponseDto>();
            }

            var cities = await _cityApiService.GetAllAsync();

            var visitorEditViewModel = new VisitorEditViewModel()
            {
                VisitorResponseDto = visitor,
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

            return View(visitorEditViewModel);
        }

        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(VisitorRequestDto visitorRequestDto)
        {
            if (ModelState.IsValid)
            {
                await _visitorApiService.AddAsync(visitorRequestDto);

                return Json(new { isValid = true, message = _localization["Visitors.Notification.Add.Text"].Value });
            }

            var cities = await _cityApiService.GetAllAsync();

            var visitorAddViewModel = new VisitorAddViewModel()
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

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Add", visitorAddViewModel) });
        }

        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(Guid id, VisitorRequestDto visitorRequestDto)
        {
            if (ModelState.IsValid)
            {
                var visitor = await _visitorApiService.GetByIdAsync<VisitorResponseDto>(id);

                if (visitor.VisitorAddressId != null)
                {
                    var visitorAddress = await _visitorAddressApiService.GetByIdAsync<VisitorAddressResponseDto>(visitor.VisitorAddressId.Value);

                    if (visitorAddress != null)
                    {
                        await _visitorAddressApiService.UpdateAsync(id, new VisitorAddressRequestDto
                        {
                            CountyId = visitorRequestDto.CountyId
                        });
                    }
                }

                else
                {
                    if (visitorRequestDto.CountyId != null)
                    {
                        await _visitorAddressApiService.AddAsync(new VisitorAddressRequestDto
                        {
                            CountyId = visitorRequestDto.CountyId
                        });
                    }
                }

                await _visitorApiService.UpdateAsync(id, visitorRequestDto);

                return Json(new { isValid = true, message = _localization["Visitors.Notification.Edit.Text"].Value });
            }

            var cities = await _cityApiService.GetAllAsync();

            var visitorEditViewModel = new VisitorEditViewModel
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

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Edit", visitorEditViewModel) });
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
