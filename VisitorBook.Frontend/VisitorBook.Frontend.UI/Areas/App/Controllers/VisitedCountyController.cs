using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using VisitorBook.Frontend.UI.Attributes;
using VisitorBook.Frontend.UI.Utilities;
using VisitorBook.Frontend.UI.Configurations;
using VisitorBook.Frontend.UI.Languages;
using VisitorBook.Frontend.UI.Services;
using VisitorBook.Frontend.UI.ViewModels;
using VisitorBook.Frontend.UI.Models.Inputs;

namespace VisitorBook.Frontend.UI.Area.App.Controllers
{
    [Area("App")]
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

        [NoDirectAccess]
        public async Task<IActionResult> Add()
        {
            var cities = await _cityApiService.GetAllAsync();

            var visitorsWithVisitorAddress = await _visitorApiService.GetAllAsync();

            var visitedCountyViewModel = new VisitedCountyViewModel()
            {
                VisitedCounty = new VisitedCountyInput(),
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

            ViewBag.BirthDates = visitorsWithVisitorAddress.Select(x => x.BirthDate).ToList();

            return View(visitedCountyViewModel);
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id)
        {
            var visitedCounty = await _visitedCountyApiService.GetByIdAsync(id);

            var counties = await _countyApiService.GetAllByCityAsync(visitedCounty.County.City.Id);

            var cities = await _cityApiService.GetAllAsync();

            var visitorsWithVisitorAddress = await _visitorApiService.GetAllAsync();

            var visitedCountyViewModel = new VisitedCountyViewModel()
            {
                VisitedCounty = new VisitedCountyInput()
                {
                    VisitorId = visitedCounty.Visitor.Id,
                    CityId = visitedCounty.County.City.Id,
                    CountyId = visitedCounty.County.Id,
                    VisitDate = visitedCounty.VisitDate
                },
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

            ViewData["BirthDates"] = visitorsWithVisitorAddress.Select(x => x.BirthDate).ToList();

            ViewData["Id"] = id;

            return View(visitedCountyViewModel);
        }

        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(VisitedCountyViewModel visitedCountyViewModel)
        {
            if (ModelState.IsValid)
            {
                await _visitedCountyApiService.AddAsync(visitedCountyViewModel.VisitedCounty);

                return Json(new { isValid = true, message = _localization["VisitedCounties.Notification.Add.Text"].Value });
            }

            var cities = await _cityApiService.GetAllAsync();

            var visitorsWithVisitorAddress = await _visitorApiService.GetAllAsync();

            visitedCountyViewModel.CityList = (cities)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   });

            visitedCountyViewModel.VisitorList = (visitorsWithVisitorAddress)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name + " " + u.Surname,
                       Value = u.Id.ToString()
                   });

            ViewData["BirthDates"] = visitorsWithVisitorAddress.Select(x => x.BirthDate).ToList();

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Add", visitedCountyViewModel) });
        }

        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id, VisitedCountyViewModel visitedCountyViewModel)
        {
            if (ModelState.IsValid)
            {
                await _visitedCountyApiService.UpdateAsync(id, visitedCountyViewModel.VisitedCounty);

                return Json(new { isValid = true, message = _localization["VisitedCounties.Notification.Edit.Text"].Value });
            }

            var cities = await _cityApiService.GetAllAsync();

            var visitorsWithVisitorAddress = await _visitorApiService.GetAllAsync();

            visitedCountyViewModel.CityList = (cities)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   });

            visitedCountyViewModel.VisitorList = (visitorsWithVisitorAddress)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name + " " + u.Surname,
                       Value = u.Id.ToString()
                   });

            ViewData["BirthDates"] = visitorsWithVisitorAddress.Select(x => x.BirthDate).ToList();

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Edit", visitedCountyViewModel) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
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
