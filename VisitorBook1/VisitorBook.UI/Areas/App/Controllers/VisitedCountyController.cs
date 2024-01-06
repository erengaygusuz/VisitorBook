using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Utilities;
using VisitorBook.UI.Configurations;
using VisitorBook.UI.Languages;
using VisitorBook.Core.ViewModels;
using VisitorBook.Core.Entities;
using VisitorBook.Core.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VisitorBook.Core.Dtos.VisitedCountyDtos;
using VisitorBook.UI.Areas.App.Controllers;
using VisitorBook.Core.Dtos.CityDtos;
using VisitorBook.Core.Dtos.CountyDtos;
using FluentValidation;
using VisitorBook.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using VisitorBook.Core.Constants;
using System.Security.Claims;
using VisitorBook.Core.Utilities.DataTablesServerSideHelpers;

namespace VisitorBook.UI.Area.App.Controllers
{
    [Authorize]
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
        private readonly IValidator<VisitedCountyViewModel> _visitedCountyViewModelValidator;

        public VisitedCountyController(IService<VisitedCounty> visitedCountyService, IService<City> cityService,
            VisitedCountyDataTablesOptions visitedCountyDataTableOptions, UserManager<User> userManager,
            IService<County> countyService, IStringLocalizer<Language> localization, RazorViewConverter razorViewConverter, IValidator<VisitedCountyViewModel> visitedCountyViewModelValidator)
        {
            _localization = localization;
            _razorViewConverter = razorViewConverter;
            _visitedCountyService = visitedCountyService;
            _cityService = cityService;
            _visitedCountyDataTableOptions = visitedCountyDataTableOptions;
            _userManager = userManager;
            _countyService = countyService;
            _visitedCountyViewModelValidator = visitedCountyViewModelValidator;
        }

        [Authorize(Permissions.VisitedCountyManagement.View)]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Permissions.VisitedCountyManagement.View)]
        [HttpPost]
        public async Task<IActionResult> GetAll()
        {
            _visitedCountyDataTableOptions.SetDataTableOptions(Request);

            PagedList<VisitedCountyResponseDto> result;

            if (User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).FirstOrDefault() == AppRoles.Visitor)
            {
                var userEmail = User.Claims.Where(x => x.Type == ClaimTypes.Email).Select(x => x.Value).FirstOrDefault();

                var user = await _userManager.FindByEmailAsync(userEmail);

                result = _visitedCountyService.GetAll<VisitedCountyResponseDto>(_visitedCountyDataTableOptions.GetDataTablesOptions(),
                    include: x => x.Include(c => c.User).Include(c => c.County).ThenInclude(c => c.City),
                    expression: x => x.UserId == user.Id);
            }

            else
            {
                result = _visitedCountyService.GetAll<VisitedCountyResponseDto>(_visitedCountyDataTableOptions.GetDataTablesOptions(),
                    include: x => x.Include(c => c.User).Include(c => c.County).ThenInclude(c => c.City));
            }

            return DataTablesResult(result);
        }

        [Authorize(Permissions.VisitedCountyManagement.Create)]
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var cityResponseDtos = await _cityService.GetAllAsync<CityResponseDto>();

            IList<User> visitorsWithVisitorAddress;

            if (User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).FirstOrDefault() == AppRoles.Visitor)
            {
                var userEmail = User.Claims.Where(x => x.Type == ClaimTypes.Email).Select(x => x.Value).FirstOrDefault();

                var user = await _userManager.FindByEmailAsync(userEmail);

                visitorsWithVisitorAddress = new List<User>()
                {
                    user
                };
            }

            else
            {
                visitorsWithVisitorAddress = await _userManager.GetUsersInRoleAsync(AppRoles.Visitor);
            }

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

        [Authorize(Permissions.VisitedCountyManagement.Edit)]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var visitedCountyResponseDto = await _visitedCountyService.GetAsync<VisitedCountyResponseDto>(u => u.Id == id, 
                include: x => x.Include(c => c.User).Include(c => c.County).ThenInclude(c => c.City));

            var countyResponseDto = await _countyService.GetAllAsync<CountyResponseDto>(u => u.CityId == visitedCountyResponseDto.County.City.Id);

            var cityResponseDtos = await _cityService.GetAllAsync<CityResponseDto>();

            IList<User> visitorsWithVisitorAddress; 

            if (User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).FirstOrDefault() == AppRoles.Visitor)
            {
                var userEmail = User.Claims.Where(x => x.Type == ClaimTypes.Email).Select(x => x.Value).FirstOrDefault();

                var user = await _userManager.FindByEmailAsync(userEmail);

                visitorsWithVisitorAddress = new List<User>()
                {
                    user
                };
            }

            else
            {
                visitorsWithVisitorAddress = await _userManager.GetUsersInRoleAsync(AppRoles.Visitor);
            }

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

        [Authorize(Permissions.VisitedCountyManagement.Create)]
        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(VisitedCountyViewModel visitedCountyViewModel)
        {
            var validationResult = await _visitedCountyViewModelValidator.ValidateAsync(visitedCountyViewModel);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);

                var cityResponseDtos = await _cityService.GetAllAsync<CityResponseDto>();

                IList<User> visitorsWithVisitorAddress;

                if (User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).FirstOrDefault() == AppRoles.Visitor)
                {
                    var userEmail = User.Claims.Where(x => x.Type == ClaimTypes.Email).Select(x => x.Value).FirstOrDefault();

                    var user = await _userManager.FindByEmailAsync(userEmail);

                    visitorsWithVisitorAddress = new List<User>()
                    {
                        user
                    };
                }

                else
                {
                    visitorsWithVisitorAddress = await _userManager.GetUsersInRoleAsync(AppRoles.Visitor);
                }

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
                
            await _visitedCountyService.AddAsync(visitedCountyViewModel.VisitedCounty);
                
            return Json(new { isValid = true, message = _localization["VisitedCounties.Notification.Add.Text"].Value });
        }

        [Authorize(Permissions.VisitedCountyManagement.Edit)]
        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(VisitedCountyViewModel visitedCountyViewModel)
        {
            var validationResult = await _visitedCountyViewModelValidator.ValidateAsync(visitedCountyViewModel);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);

                var cityResponseDtos = await _cityService.GetAllAsync<CityResponseDto>();

                IList<User> visitorsWithVisitorAddress;

                if (User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).FirstOrDefault() == AppRoles.Visitor)
                {
                    var userEmail = User.Claims.Where(x => x.Type == ClaimTypes.Email).Select(x => x.Value).FirstOrDefault();

                    var user = await _userManager.FindByEmailAsync(userEmail);

                    visitorsWithVisitorAddress = new List<User>()
                    {
                        user
                    };
                }

                else
                {
                    visitorsWithVisitorAddress = await _userManager.GetUsersInRoleAsync(AppRoles.Visitor);
                }

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
                
            await _visitedCountyService.UpdateAsync(visitedCountyViewModel.VisitedCounty);
                
            return Json(new { isValid = true, message = _localization["VisitedCounties.Notification.Edit.Text"].Value });
        }

        [Authorize(Permissions.VisitedCountyManagement.Delete)]
        [HttpGet]
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

            return Json(new { message = _localization["VisitedCounties.Notification.UnSuccessfullDelete.Text"].Value });
        }
    }
}
