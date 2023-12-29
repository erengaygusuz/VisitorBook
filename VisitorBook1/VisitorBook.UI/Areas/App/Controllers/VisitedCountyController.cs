using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using VisitorBook.UI.Attributes;
using VisitorBook.Core.Utilities;
using VisitorBook.UI.Configurations;
using VisitorBook.UI.Languages;
using VisitorBook.UI.ViewModels;
using VisitorBook.Core.Entities;
using VisitorBook.Core.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VisitorBook.Core.Dtos.VisitedCountyDtos;
using VisitorBook.UI.Areas.App.Controllers;
using VisitorBook.Core.Dtos.CityDtos;
using VisitorBook.Core.Dtos.CountyDtos;

namespace VisitorBook.UI.Area.App.Controllers
{
    [Area("App")]
    public class VisitedCountyController : BaseController
    {
        private readonly IStringLocalizer<Language> _localization;
        private readonly RazorViewConverter _razorViewConverter;
        private readonly IService<VisitedCounty> _visitedCountyService;
        private readonly IService<City> _cityService;
        private readonly IService<County> _countyService;
        private readonly UserManager<User> _userManager;
        private readonly VisitedCountyDataTablesOptions _visitedCountyDataTableOptions;       

        public VisitedCountyController(IService<VisitedCounty> visitedCountyService, IService<City> cityService,
            VisitedCountyDataTablesOptions visitedCountyDataTableOptions, UserManager<User> userManager,
            IService<County> countyService, IStringLocalizer<Language> localization, RazorViewConverter razorViewConverter)
        {
            _localization = localization;
            _razorViewConverter = razorViewConverter;
            _visitedCountyService = visitedCountyService;
            _cityService = cityService;
            _visitedCountyDataTableOptions = visitedCountyDataTableOptions;
            _userManager = userManager;
            _countyService = countyService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetAll()
        {
            _visitedCountyDataTableOptions.SetDataTableOptions(Request);

            var result = _visitedCountyService.GetAll<VisitedCountyResponseDto>(_visitedCountyDataTableOptions.GetDataTablesOptions(), 
                include: x => x.Include(c => c.User).Include(c => c.County).ThenInclude(c => c.City));

            return DataTablesResult(result);
        }

        [NoDirectAccess]
        public async Task<IActionResult> Add()
        {
            var cityResponseDtos = await _cityService.GetAllAsync<CityResponseDto>();

            var visitorsWithVisitorAddress = await _userManager.Users.ToListAsync();

            var visitedCountyViewModel = new VisitedCountyViewModel()
            {
                VisitedCounty = new VisitedCountyRequestDto(),
                CountyList = new List<SelectListItem>(),
                CityList = (cityResponseDtos)
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
            var visitedCountyResponseDto = await _visitedCountyService.GetAsync<VisitedCountyResponseDto>(u => u.Id == id, 
                include: x => x.Include(c => c.User).Include(c => c.County).ThenInclude(c => c.City));

            var countyResponseDto = await _countyService.GetAllAsync<CountyResponseDto>(u => u.CityId == visitedCountyResponseDto.County.City.Id);

            var cityResponseDtos = await _cityService.GetAllAsync<CityResponseDto>();

            var visitorsWithVisitorAddress = await _userManager.Users.ToListAsync();

            var visitedCountyViewModel = new VisitedCountyViewModel()
            {
                VisitedCounty = new VisitedCountyRequestDto()
                {
                    Id = id,
                    UserId = visitedCountyResponseDto.User.Id,
                    CityId = visitedCountyResponseDto.County.City.Id,
                    CountyId = visitedCountyResponseDto.County.Id,
                    VisitDate = visitedCountyResponseDto.VisitDate
                },
                CountyList = (countyResponseDto)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                CityList = (cityResponseDtos)
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

        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(VisitedCountyViewModel visitedCountyViewModel)
        {
            if (ModelState.IsValid)
            {
                await _visitedCountyService.AddAsync(visitedCountyViewModel.VisitedCounty);

                return Json(new { isValid = true, message = _localization["VisitedCounties.Notification.Add.Text"].Value });
            }

            var cityResponseDtos = await _cityService.GetAllAsync<CityResponseDto>();

            var visitorsWithVisitorAddress = await _userManager.Users.ToListAsync();

            visitedCountyViewModel.CityList = (cityResponseDtos)
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
        public async Task<IActionResult> EditPost(VisitedCountyViewModel visitedCountyViewModel)
        {
            if (ModelState.IsValid)
            {
                await _visitedCountyService.UpdateAsync(visitedCountyViewModel.VisitedCounty);

                return Json(new { isValid = true, message = _localization["VisitedCounties.Notification.Edit.Text"].Value });
            }

            var cityResponseDtos = await _cityService.GetAllAsync<CityResponseDto>();

            var visitorsWithVisitorAddress = await _userManager.Users.ToListAsync();

            visitedCountyViewModel.CityList = (cityResponseDtos)
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
        public async Task<IActionResult> Delete(int id)
        {
            var visitedCountyResponseDto = await _visitedCountyService.GetAsync<VisitedCountyResponseDto>(u => u.Id == id, 
                include: x => x.Include(c => c.User).Include(c => c.County).ThenInclude(c => c.City));

            if (visitedCountyResponseDto == null)
            {
                return NotFound();
            }

            var result = await _visitedCountyService.RemoveAsync(visitedCountyResponseDto);

            if (result)
            {
                return Json(new { message = _localization["VisitedCounties.Notification.SuccessfullDelete.Text"].Value });
            }

            return BadRequest(new { message = _localization["VisitedCounties.Notification.UnSuccessfullDelete.Text"].Value });
        }
    }
}
