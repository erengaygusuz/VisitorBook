﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using VisitorBook.UI.Attributes;
using VisitorBook.UI.Utilities;
using VisitorBook.UI.Configurations;
using VisitorBook.UI.Languages;
using VisitorBook.UI.Services;
using VisitorBook.UI.ViewModels;
using VisitorBook.UI.Models.Inputs;

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

            return View(visitedCountyViewModel);
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(Guid id)
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

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Add", visitedCountyViewModel) });
        }

        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(Guid id, VisitedCountyViewModel visitedCountyViewModel)
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

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Edit", visitedCountyViewModel) });
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
