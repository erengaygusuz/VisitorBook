using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Constants;
using VisitorBook.Core.Dtos.RegisterApplicationDto;
using VisitorBook.Core.Entities;
using VisitorBook.Core.Enums;
using VisitorBook.Core.Utilities;
using VisitorBook.Core.ViewModels;
using VisitorBook.UI.Configurations;
using VisitorBook.UI.Languages;

namespace VisitorBook.UI.Areas.App.Controllers
{
    [Authorize]
    [Area("App")]
    public class RegisterApplicationController : BaseController
    {
        private readonly IService<RegisterApplication> _registerApplicationService;
        private readonly IValidator<RegisterApplicationViewModel> _registerApplicationViewModelValidator;
        private readonly IStringLocalizer<Language> _localization;
        private readonly RazorViewConverter _razorViewConverter;
        private readonly RegisterApplicationDataTablesOptions _registerApplicationDataTablesOptions;

        public RegisterApplicationController(IService<RegisterApplication> registerApplicationService, 
            IValidator<RegisterApplicationViewModel> registerApplicationViewModelValidator, IStringLocalizer<Language> localization,
            RazorViewConverter razorViewConverter, RegisterApplicationDataTablesOptions registerApplicationDataTablesOptions)
        {
            _registerApplicationService = registerApplicationService;
            _registerApplicationViewModelValidator = registerApplicationViewModelValidator;
            _localization = localization;
            _razorViewConverter = razorViewConverter;
            _registerApplicationDataTablesOptions = registerApplicationDataTablesOptions;
        }

        [Authorize(Permissions.RegisterApplicationManagement.View)]
        [HttpPost]
        public IActionResult PendingApplications()
        {
            _registerApplicationDataTablesOptions.SetDataTableOptions(Request);

            var result = _registerApplicationService.GetAll<RegisterApplicationResponseDto>(_registerApplicationDataTablesOptions.GetDataTablesOptions(),
                include: x => x.Include(c => c.User),
                expression: x => x.Status == RegisterApplicationStatus.Pending);

            return DataTablesResult(result);
        }

        [Authorize(Permissions.RegisterApplicationManagement.View)]
        [HttpGet]
        public async Task<IActionResult> PendingApplication(int id)
        {
            var registerApplicationResponseDto = await _registerApplicationService.GetAsync<RegisterApplicationResponseDto>(x => x.Id == id,
                include: x => x.Include(c => c.User));

            var registerApplicationViewModel = new RegisterApplicationViewModel()
            {
                RegisterApplicationStatusList = Enum.GetNames(typeof(RegisterApplicationStatus))
                   .Select(u => new SelectListItem
                   {
                       Text = _localization["Enum.RegisterApplicationStatus." + u + ".Text"].Value,
                       Value = u.ToString()
                   }),
                RegisterApplication = new RegisterApplicationRequestDto()
                {
                    Id = id,
                    Name = registerApplicationResponseDto.User.Name,
                    Surname = registerApplicationResponseDto.User.Surname,
                    Email = registerApplicationResponseDto.User.Email,
                    Username = registerApplicationResponseDto.User.Username,
                    Explanation = registerApplicationResponseDto.Explanation,
                    Status = registerApplicationResponseDto.Status
                }
            };

            return View(registerApplicationViewModel);
        }

        [Authorize(Permissions.RegisterApplicationManagement.Edit)]
        [ActionName("PendingApplication")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PendingApplicationPost(RegisterApplicationViewModel registerApplicationViewModel)
        {
            var validationResult = await _registerApplicationViewModelValidator.ValidateAsync(registerApplicationViewModel);

            if (!validationResult.IsValid)
            {
                registerApplicationViewModel.RegisterApplicationStatusList = Enum.GetNames(typeof(RegisterApplicationStatus))
                   .Select(u => new SelectListItem
                   {
                       Text = _localization["Enum.RegisterApplicationStatus." + u + ".Text"].Value,
                       Value = u.ToString()
                   });

                return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "PendingApplication", registerApplicationViewModel) });
            }

            await _registerApplicationService.UpdateAsync(registerApplicationViewModel.RegisterApplication);

            return Json(new { isValid = true, message = _localization["RegisterApplications.Notification.Edit.Text"].Value });
        }

        [Authorize(Permissions.RegisterApplicationManagement.View)]
        [HttpPost]
        public IActionResult PastApplications()
        {
            _registerApplicationDataTablesOptions.SetDataTableOptions(Request);

            var result = _registerApplicationService.GetAll<RegisterApplicationResponseDto>(_registerApplicationDataTablesOptions.GetDataTablesOptions(), 
                include: x => x.Include(c => c.User),
                expression: x => x.Status != RegisterApplicationStatus.Pending);

            return DataTablesResult(result);
        }

        [Authorize(Permissions.RegisterApplicationManagement.View)]
        [HttpGet]
        public async Task<IActionResult> PastApplication(int id)
        {
            var registerApplicationResponseDto = await _registerApplicationService.GetAsync<RegisterApplicationResponseDto>(x => x.Id == id,
               include: x => x.Include(c => c.User));

            var registerApplicationViewModel = new RegisterApplicationViewModel()
            {
                RegisterApplicationStatusList = Enum.GetNames(typeof(RegisterApplicationStatus))
                   .Select(u => new SelectListItem
                   {
                       Text = _localization["Enum.RegisterApplicationStatus." + u + ".Text"].Value,
                       Value = u.ToString()
                   }),
                RegisterApplication = new RegisterApplicationRequestDto()
                {
                    Id = id,
                    Name = registerApplicationResponseDto.User.Name,
                    Surname = registerApplicationResponseDto.User.Surname,
                    Email = registerApplicationResponseDto.User.Email,
                    Username = registerApplicationResponseDto.User.Username,
                    Explanation = registerApplicationResponseDto.Explanation,
                    Status = registerApplicationResponseDto.Status
                }
            };

            return View(registerApplicationViewModel);
        }
    }
}
