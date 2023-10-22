using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Drawing.Printing;
using VisitorBook.Core.Abstract;
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
        private readonly IStringLocalizer<Language> _localization;

        [BindProperty]
        public VisitorViewModel VisitorViewModel { get; set; }

        public VisitorController(IService<County> countyService, IService<City> cityService,
            IService<Visitor> visitorService, IStringLocalizer<Language> localization)
        {
            _countyService = countyService;
            _cityService = cityService;
            _visitorService = visitorService;
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

            var visitors = await _visitorService.GetAllAsync(
                    page: page, pageSize: pageSize,
                    expression: (!string.IsNullOrEmpty(searchValue)) ? vc => vc.Name.Contains(searchValue) : null,
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
                    Gender = u.Gender.ToString(),
                    BirthDate = u.BirthDate
                })
            });
        }

        public async Task<IActionResult> AddOrEdit(int id)
        {
            VisitorViewModel = new VisitorViewModel()
            {
                Visitor = new Visitor(),
                GenderList = new List<Gender> { Gender.Man, Gender.Woman }
                    .Select(u => new SelectListItem
                    {
                        Text = u.ToString(),
                        Value = u.ToString()
                    }),
                CityList = _cityService.GetAllAsync().GetAwaiter().GetResult().ToList()
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                CountyList = new List<SelectListItem>()
            };

            if (id == 0)
            {
                // create
                return View(VisitorViewModel);
            }

            else
            {
                // update
                VisitorViewModel.Visitor = await _visitorService.GetAsync(u => u.Id == id, include: u => u.Include(a => a.VisitorAddress).ThenInclude(b => b.County));

                if (VisitorViewModel.Visitor.VisitorAddress != null)
                {
                    VisitorViewModel.CountyList = _countyService.GetAllAsync(u => u.CityId == VisitorViewModel.Visitor.VisitorAddress.County.CityId).GetAwaiter().GetResult().ToList()
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   });
                }

                return View(VisitorViewModel);
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
                    await _visitorService.AddAsync(VisitorViewModel.Visitor);

                    return Json(new { isValid = true, message = _localization["Visitors.Notification.Add.Text"].Value });
                }

                else
                {
                    var visitor = await _visitorService.GetAsync(u => u.Id == id, include: u => u.Include(a => a.VisitorAddress));

                    visitor.Name = VisitorViewModel.Visitor.Name;
                    visitor.Surname = VisitorViewModel.Visitor.Surname;
                    visitor.BirthDate = VisitorViewModel.Visitor.BirthDate;
                    visitor.Gender = VisitorViewModel.Visitor.Gender;

                    if (VisitorViewModel.Visitor.VisitorAddress != null)
                    {
                        var newCountyWithCity = await _countyService.GetAsync(u => u.Id == VisitorViewModel.Visitor.VisitorAddress.CountyId, include: u => u.Include(a => a.City));

                        if (visitor.VisitorAddress != null)
                        {
                            visitor.VisitorAddress.CountyId = newCountyWithCity.Id;
                        }

                        else
                        {
                            visitor.VisitorAddress = new VisitorAddress
                            {
                                CountyId = newCountyWithCity.Id
                            };
                        }
                    }

                    await _visitorService.UpdateAsync(visitor);

                    return Json(new { isValid = true, message = _localization["Visitors.Notification.Edit.Text"].Value });
                }
            }

            return Json(new { isValid = false, html = RazorViewConverter.GetStringFromRazorView(this, "AddOrEdit", VisitorViewModel.Visitor) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var visitor = await _visitorService.GetAsync(u => u.Id == id);

            var visitorName = visitor.Name + " " + visitor.Surname;

            if (visitor != null)
            {
                await _visitorService.RemoveAsync(visitor);
            }

            return Json(new { message = _localization["Visitors.Notification.Delete.Text"].Value });
        }
    }
}
