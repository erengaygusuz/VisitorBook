﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Abstract;
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

        [BindProperty]
        public VisitedCountyViewModel VisitedCountyViewModel { get; set; }

        public VisitedCountyController(IService<County> countyService, IService<Visitor> visitorService,
                                       IService<VisitedCounty> visitedCountyService, IService<City> cityService, IStringLocalizer<Language> localization)
        {
            _countyService = countyService;
            _visitorService = visitorService;
            _visitedCountyService = visitedCountyService;
            _cityService = cityService;
            _localization = localization;
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
                    expression: (!string.IsNullOrEmpty(searchValue)) ? vc => vc.VisitorAddress.Visitor.Name.Contains(searchValue) : null,
                    include: u => u.Include(a => a.VisitorAddress).ThenInclude(a => a.Visitor).Include(a => a.County).ThenInclude(b => b.City),
                    orderBy: (sortColumnDirection == "asc") ?
                        (o =>
                        o switch
                        {
                            _ when sortColumnIndex == "0" => o.OrderBy(s => s.VisitorAddress.Visitor.Name + " " + s.VisitorAddress.Visitor.Surname),
                            _ when sortColumnIndex == "1" => o.OrderBy(s => s.County.Name),
                            _ when sortColumnIndex == "2" => o.OrderBy(s => s.County.City.Name),
                            _ when sortColumnIndex == "3" => o.OrderBy(s => s.VisitDate),
                            _ => o.OrderBy(s => s.VisitorAddress.Visitor.Name + " " + s.VisitorAddress.Visitor.Surname)
                        })
                         :
                        (o =>
                        o switch
                        {
                            _ when sortColumnIndex == "0" => o.OrderByDescending(s => s.VisitorAddress.Visitor.Name + " " + s.VisitorAddress.Visitor.Surname),
                            _ when sortColumnIndex == "1" => o.OrderByDescending(s => s.County.Name),
                            _ when sortColumnIndex == "2" => o.OrderByDescending(s => s.County.City.Name),
                            _ when sortColumnIndex == "3" => o.OrderByDescending(s => s.VisitDate),
                            _ => o.OrderByDescending(s => s.VisitorAddress.Visitor.Name + " " + s.VisitorAddress.Visitor.Surname)
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

        public async Task<IActionResult> AddOrEdit(int id)
        {
            VisitedCountyViewModel = new VisitedCountyViewModel()
            {
                VisitedCounty = new VisitedCounty(),
                CountyList = new List<SelectListItem>(),
                CityList = (await _cityService.GetAllAsync())
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                VisitorList = (await _visitorService.GetAllAsync(v => v.VisitorAddress != null, include: u => u.Include(a => a.VisitorAddress)))
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name + " " + u.Surname,
                       Value = u.Id.ToString()
                   })
            };

            if (id == 0)
            {
                // create
                return View(VisitedCountyViewModel);
            }

            else
            {
                // update
                VisitedCountyViewModel.VisitedCounty = await _visitedCountyService.GetAsync(u => u.Id == id, include: a => a.Include(b => b.County));

                VisitedCountyViewModel.CountyList = (await _countyService.GetAllAsync(u => u.CityId == VisitedCountyViewModel.VisitedCounty.County.CityId))
                  .Select(u => new SelectListItem
                  {
                      Text = u.Name,
                      Value = u.Id.ToString()
                  });

                return View(VisitedCountyViewModel);
            }
        }

        [ActionName("AddOrEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditPost(int id)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    await _visitedCountyService.AddAsync(VisitedCountyViewModel.VisitedCounty);

                    return Json(new { isValid = true, message = _localization["VisitedCounties.Notification.Add.Text"].Value });
                }

                else
                {
                    var visitedCounty = await _visitedCountyService.GetAsync(u => u.Id == id);

                    visitedCounty.VisitDate = VisitedCountyViewModel.VisitedCounty.VisitDate;
                    visitedCounty.VisitorAddress = VisitedCountyViewModel.VisitedCounty.VisitorAddress;

                    var newCountyWithCity = await _countyService.GetAsync(u => u.Id == VisitedCountyViewModel.VisitedCounty.CountyId, include: u => u.Include(a => a.City));

                    visitedCounty.CountyId = newCountyWithCity.Id;

                    await _visitedCountyService.UpdateAsync(visitedCounty);

                    return Json(new { isValid = true, message = _localization["VisitedCounties.Notification.Edit.Text"].Value });
                }
            }

            return Json(new { isValid = false, html = RazorViewConverter.GetStringFromRazorView(this, "AddOrEdit", VisitedCountyViewModel.VisitedCounty) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var visitedCounty = await _visitedCountyService.GetAsync(u => u.Id == id);

            var visitor = await _visitorService.GetAsync(u => u.Id == visitedCounty.VisitorAddress.Visitor.Id);
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
