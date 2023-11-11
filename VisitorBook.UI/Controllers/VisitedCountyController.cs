using Microsoft.AspNetCore.Mvc;
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
        private readonly RazorViewConverter _razorViewConverter;

        [BindProperty]
        public VisitedCountyViewModel VisitedCountyViewModel { get; set; }

        public VisitedCountyController(IService<County> countyService, IService<Visitor> visitorService,
                                       IService<VisitedCounty> visitedCountyService, IService<City> cityService, IStringLocalizer<Language> localization, RazorViewConverter razorViewConverter)
        {
            _countyService = countyService;
            _visitorService = visitorService;
            _visitedCountyService = visitedCountyService;
            _cityService = cityService;
            _localization = localization;
            _razorViewConverter = razorViewConverter;
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
                VisitorList = (await _visitorService.GetAllAsync(v => v.VisitorAddressId != null))
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name + " " + u.Surname,
                       Value = u.Id.ToString()
                   })
            };

            return View(VisitedCountyViewModel);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var visitedCounty = await _visitedCountyService.GetAsync(u => u.Id == id, include: a => a.Include(b => b.County));
            var countyList = await _countyService.GetAllAsync(u => u.CityId == visitedCounty.County.CityId);

            VisitedCountyViewModel = new VisitedCountyViewModel()
            {
                VisitedCounty = visitedCounty,
                CountyList = countyList
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                CityList = (await _cityService.GetAllAsync())
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                VisitorList = (await _visitorService.GetAllAsync(v => v.VisitorAddress != null))
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name + " " + u.Surname,
                       Value = u.Id.ToString()
                   })
            };

            return View(VisitedCountyViewModel);
        }

        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost()
        {
            if (ModelState.IsValid)
            {
                await _visitedCountyService.AddAsync(VisitedCountyViewModel.VisitedCounty);

                return Json(new { isValid = true, message = _localization["VisitedCounties.Notification.Add.Text"].Value });
            }

            VisitedCountyViewModel.VisitorList = (await _visitorService.GetAllAsync(v => v.VisitorAddressId != null))
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name + " " + u.Surname,
                       Value = u.Id.ToString()
                   });

            VisitedCountyViewModel.CityList = (await _cityService.GetAllAsync())
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   });

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Add", VisitedCountyViewModel) });
        }

        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost()
        {
            if (ModelState.IsValid)
            {
                var visitedCounty = await _visitedCountyService.GetAsync(u => u.Id == VisitedCountyViewModel.VisitedCounty.Id);

                visitedCounty.VisitDate = VisitedCountyViewModel.VisitedCounty.VisitDate;
                visitedCounty.VisitorId = VisitedCountyViewModel.VisitedCounty.VisitorId;

                var newCounty = await _countyService.GetAsync(u => u.Id == VisitedCountyViewModel.VisitedCounty.CountyId);

                visitedCounty.CountyId = newCounty.Id;

                await _visitedCountyService.UpdateAsync(visitedCounty);

                return Json(new { isValid = true, message = _localization["VisitedCounties.Notification.Edit.Text"].Value });
            }

            VisitedCountyViewModel.VisitorList = (await _visitorService.GetAllAsync(v => v.VisitorAddress != null))
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name + " " + u.Surname,
                       Value = u.Id.ToString()
                   });

            VisitedCountyViewModel.CityList = (await _cityService.GetAllAsync())
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   });

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Edit", VisitedCountyViewModel) });
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
