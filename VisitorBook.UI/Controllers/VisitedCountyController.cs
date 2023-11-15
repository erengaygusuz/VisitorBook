using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Dtos.VisitedCountyDtos;
using VisitorBook.Core.Utilities;
using VisitorBook.UI.Configurations;
using VisitorBook.UI.Languages;
using VisitorBook.UI.Services;
using VisitorBook.UI.ViewModels;

namespace VisitorBook.UI.Controllers
{
    public class VisitedCountyController : Controller
    {
        private readonly IStringLocalizer<Language> _localization;
        private readonly RazorViewConverter _razorViewConverter;
        private readonly VisitedCountyApiService _visitedCountyApiService;
        private readonly CityApiService _cityApiService;
        private readonly CountyApiService _countyApiService;
        private readonly VisitorApiService _visitorApiService;
        private readonly VisitedCountyDataTablesOptions _visitedCountyDataTableOptions;       

        public VisitedCountyController(VisitedCountyApiService visitedCountyApiService, CityApiService cityApiService,
            VisitedCountyDataTablesOptions visitedCountyDataTableOptions, VisitorApiService visitorApiService,
            CountyApiService countyApiService, IStringLocalizer<Language> localization, RazorViewConverter razorViewConverter)
        {
            _localization = localization;
            _razorViewConverter = razorViewConverter;
            _visitedCountyApiService = visitedCountyApiService;
            _cityApiService = cityApiService;
            _visitedCountyDataTableOptions = visitedCountyDataTableOptions;
            _visitorApiService = visitorApiService;
            _countyApiService = countyApiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAll()
        {
            _visitedCountyDataTableOptions.SetDataTableOptions(Request);

            var result = await _visitedCountyApiService.GetTableData(_visitedCountyDataTableOptions.GetDataTablesOptions());

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

            var visitorsWithVisitorAddress = await _visitorApiService.GetAllAsync();

            var visitedCountyAddViewModel = new VisitedCountyAddViewModel()
            {
                VisitedCountyRequestDto = new VisitedCountyRequestDto(),
                CountyList = new List<SelectListItem>(),
                CityList = (cities)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                VisitorList = (visitorsWithVisitorAddress)
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
            var visitedCounty = await _visitedCountyApiService.GetByIdAsync<VisitedCountyResponseDto>(id);

            var counties = await _countyApiService.GetAllByCityAsync(visitedCounty.CityId);

            var cities = await _cityApiService.GetAllAsync();

            var visitorsWithVisitorAddress = await _visitorApiService.GetAllAsync();

            var visitedCountyEditViewModel = new VisitedCountyEditViewModel()
            {
                VisitedCountyResponseDto = visitedCounty,
                CountyList = (counties)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                CityList = (cities)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                VisitorList = (visitorsWithVisitorAddress)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name + " " + u.Surname,
                       Value = u.Id.ToString()
                   })
            };

            return View(visitedCountyEditViewModel);
        }

        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(VisitedCountyRequestDto visitedCountyAddRequestDto)
        {
            if (ModelState.IsValid)
            {
                await _visitedCountyApiService.AddAsync(visitedCountyAddRequestDto);

                return Json(new { isValid = true, message = _localization["VisitedCounties.Notification.Add.Text"].Value });
            }

            var cities = await _cityApiService.GetAllAsync();

            var visitorsWithVisitorAddress = await _visitorApiService.GetAllAsync();

            var visitedCountyAddViewModel = new VisitedCountyEditViewModel()
            {
                CityList = (cities)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                VisitorList = (visitorsWithVisitorAddress)
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
        public async Task<IActionResult> EditPost(Guid id, VisitedCountyRequestDto visitedCountyRequestDto)
        {
            if (ModelState.IsValid)
            {
                await _visitedCountyApiService.UpdateAsync(id, visitedCountyRequestDto);

                return Json(new { isValid = true, message = _localization["VisitedCounties.Notification.Edit.Text"].Value });
            }

            var cities = await _cityApiService.GetAllAsync();

            var visitorsWithVisitorAddress = await _visitorApiService.GetAllAsync();

            var visitedCountyEditViewModel = new VisitedCountyEditViewModel()
            {
                CityList = (cities)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                VisitorList = (visitorsWithVisitorAddress)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name + " " + u.Surname,
                       Value = u.Id.ToString()
                   })
            };

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Edit", visitedCountyEditViewModel) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _visitedCountyApiService.RemoveAsync(id);

            if (result)
            {
                return Json(new { message = _localization["VisitedCounties.Notification.SuccessfullDelete.Text"].Value });
            }

            return BadRequest(new { message = _localization["VisitedCounties.Notification.UnSuccessfullDelete.Text"].Value });
        }
    }
}
