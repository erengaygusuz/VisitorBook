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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var visitedCountys = await _visitedCountyService.GetAllAsync(include: u => u.Include(a => a.Visitor).Include(a => a.County).ThenInclude(b => b.City));

            return Json(new
            {
                data = visitedCountys
            });
        }

        public async Task<IActionResult> AddOrEdit(int id)
        {
            VisitedCountyViewModel = new VisitedCountyViewModel()
            {
                VisitedCounty = new VisitedCounty(),
                CountyList = new List<SelectListItem>(),
                CityList = _cityService.GetAllAsync().GetAwaiter().GetResult().ToList()
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                VisitorList = _visitorService.GetAllAsync(v => v.VisitorAddress != null).GetAwaiter().GetResult().ToList()
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
                VisitedCountyViewModel.VisitedCounty = _visitedCountyService.GetAsync(u => u.Id == id).GetAwaiter().GetResult();

                VisitedCountyViewModel.CountyList = _countyService.GetAllAsync(u => u.CityId == VisitedCountyViewModel.VisitedCounty.County.CityId).GetAwaiter().GetResult().ToList()
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

                    return Json(new { isValid = true, message = _localization["VisitedCountys.Notification.Add.Text"].Value });
                }

                else
                {
                    var visitedCounty = await _visitedCountyService.GetAsync(u => u.Id == id);

                    visitedCounty.VisitDate = VisitedCountyViewModel.VisitedCounty.VisitDate;
                    visitedCounty.VisitorId = VisitedCountyViewModel.VisitedCounty.VisitorId;

                    var newCountyWithCity = await _countyService.GetAsync(u => u.Id == VisitedCountyViewModel.VisitedCounty.CountyId, include: u => u.Include(a => a.City));

                    visitedCounty.CountyId = newCountyWithCity.Id;

                    await _visitedCountyService.UpdateAsync(visitedCounty);

                    return Json(new { isValid = true, message = _localization["VisitedCountys.Notification.Edit.Text"].Value });
                }
            }

            return Json(new { isValid = false, html = RazorViewConverter.GetStringFromRazorView(this, "AddOrEdit", VisitedCountyViewModel.VisitedCounty) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
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
