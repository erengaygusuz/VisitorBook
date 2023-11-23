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
using VisitorBook.Frontend.UI.Models.Outputs;

namespace VisitorBook.Frontend.UI.Controllers
{
    public class VisitorController : Controller
    {
        private readonly CountyApiService _countyApiService;
        private readonly CityApiService _cityApiService;
        private readonly VisitorApiService _visitorApiService;
        private readonly IStringLocalizer<Language> _localization;
        private readonly RazorViewConverter _razorViewConverter;
        private readonly VisitorDataTablesOptions _visitorDataTableOptions;

        public VisitorController(CountyApiService countyApiService, CityApiService cityApiService,
            VisitorApiService visitorApiService, IStringLocalizer<Language> localization,
            RazorViewConverter razorViewConverter, 
            VisitorDataTablesOptions visitorDataTableOptions)
        {
            _countyApiService = countyApiService;
            _cityApiService = cityApiService;
            _visitorApiService = visitorApiService;
            _localization = localization;
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

            result.Data = result.Data.Select(v => new VisitorOutput
            {
                Id = v.Id,
                Name = v.Name,
                Surname = v.Surname,
                BirthDate = v.BirthDate,
                Gender = _localization["Enum.Gender." + v.Gender + ".Text"].Value,
                VisitorAddress = v.VisitorAddress
            }).ToList();

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

            var visitorViewModel = new VisitorViewModel()
            {
                Visitor = new VisitorInput(),
                GenderList = new List<string> { "Male", "Female" }
                   .Select(u => new SelectListItem
                   {
                       Text = _localization["Enum.Gender." + u + ".Text"].Value,
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

            return View(visitorViewModel);
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(Guid id)
        {
            var visitor = await _visitorApiService.GetByIdAsync(id);

            List<CountyOutput> counties;

            if (visitor.VisitorAddress != null)
            {
                counties = await _countyApiService.GetAllByCityAsync(visitor.VisitorAddress.CityId);
            }

            else
            {
                counties = new List<CountyOutput>();
            }

            var cities = await _cityApiService.GetAllAsync();

            var visitorViewModel = new VisitorViewModel()
            {
                Visitor = new VisitorInput()
                {
                    Name = visitor.Name,
                    Surname = visitor.Surname,
                    BirthDate = visitor.BirthDate,
                    Gender = visitor.Gender,
                    VisitorAddress = visitor.VisitorAddress != null ? 
                    new VisitorAddressInput()
                    {
                        Id = visitor.VisitorAddress.Id,
                        CityId = visitor.VisitorAddress.CityId,
                        CountyId = visitor.VisitorAddress.CountyId
                    } 
                    : 
                    new VisitorAddressInput()
                },
                GenderList = new List<string> { "Male", "Female" }
                   .Select(u => new SelectListItem
                   {
                       Text = _localization["Enum.Gender." + u + ".Text"].Value,
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

            ViewData["Id"] = id;

            return View(visitorViewModel);
        }

        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(VisitorViewModel visitorViewModel)
        {
            if (ModelState.IsValid)
            {
                await _visitorApiService.AddAsync(visitorViewModel.Visitor);

                return Json(new { isValid = true, message = _localization["Visitors.Notification.Add.Text"].Value });
            }

            var cities = await _cityApiService.GetAllAsync();

            visitorViewModel.CityList = (cities)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   });

            visitorViewModel.GenderList = new List<string> { "Male", "Female" }
                   .Select(u => new SelectListItem
                   {
                       Text = _localization["Enum.Gender." + u + ".Text"].Value,
                       Value = u.ToString()
                   });

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Add", visitorViewModel) });
        }

        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(Guid id, VisitorViewModel visitorViewModel)
        {
            if (ModelState.IsValid)
            {
                await _visitorApiService.UpdateAsync(id, visitorViewModel.Visitor);

                return Json(new { isValid = true, message = _localization["Visitors.Notification.Edit.Text"].Value });
            }

            var cities = await _cityApiService.GetAllAsync();

            visitorViewModel.CityList = (cities)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   });

            visitorViewModel.GenderList = new List<string> { "Male", "Female" }
                   .Select(u => new SelectListItem
                   {
                       Text = _localization["Enum.Gender." + u + ".Text"].Value,
                       Value = u.ToString()
                   });

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Edit", visitorViewModel) });
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
