﻿using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Constants;
using VisitorBook.Core.Dtos.RegisterApplicationDto;
using VisitorBook.Core.Dtos.UserDtos;
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
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _environment;

        public RegisterApplicationController(IService<RegisterApplication> registerApplicationService, 
            IValidator<RegisterApplicationViewModel> registerApplicationViewModelValidator, IStringLocalizer<Language> localization,
            RazorViewConverter razorViewConverter, RegisterApplicationDataTablesOptions registerApplicationDataTablesOptions, 
            UserManager<User> userManager, IEmailService emailService, IWebHostEnvironment environment)
        {
            _registerApplicationService = registerApplicationService;
            _registerApplicationViewModelValidator = registerApplicationViewModelValidator;
            _localization = localization;
            _razorViewConverter = razorViewConverter;
            _registerApplicationDataTablesOptions = registerApplicationDataTablesOptions;
            _userManager = userManager;
            _emailService = emailService;
            _environment = environment;
        }

        [Authorize(Permissions.RegisterApplicationManagement.View)]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Permissions.RegisterApplicationManagement.View)]
        [HttpPost]
        public IActionResult GetAll()
        {
            _registerApplicationDataTablesOptions.SetDataTableOptions(Request);

            var result = _registerApplicationService.GetAll<RegisterApplicationResponseDto>(_registerApplicationDataTablesOptions.GetDataTablesOptions(),
                include: x => x.Include(c => c.User));

            return DataTablesResult(result);
        }

        [Authorize(Permissions.RegisterApplicationManagement.View)]
        [HttpGet]
        public async Task<IActionResult> Application(int id)
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
                RegisterApplication = registerApplicationResponseDto
            };

            return View(registerApplicationViewModel);
        }

        [Authorize(Permissions.RegisterApplicationManagement.Edit)]
        [ActionName("Application")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplicationPost(RegisterApplicationViewModel registerApplicationViewModel)
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

                return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Application", registerApplicationViewModel) });
            }

            var registerApplication = await _registerApplicationService.GetAsync<RegisterApplicationResponseDto>(x => x.Id == registerApplicationViewModel.RegisterApplication.Id);

            registerApplication.Status = registerApplicationViewModel.RegisterApplication.Status;

            await _registerApplicationService.UpdateAsync(new RegisterApplicationRequestDto
            {
                Id = registerApplicationViewModel.RegisterApplication.Id,
                UserId = registerApplicationViewModel.RegisterApplication.User.Id,
                Explanation = registerApplicationViewModel.RegisterApplication.Explanation,
                Status = registerApplicationViewModel.RegisterApplication.Status
            });

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == registerApplicationViewModel.RegisterApplication.User.Id);

            if (registerApplicationViewModel.RegisterApplication.Status == RegisterApplicationStatus.Approved.ToString())
            {
                user.EmailConfirmed = true;

                var result = await _userManager.UpdateAsync(user);

                var pathToFile = _environment.WebRootPath
                                    + Path.DirectorySeparatorChar.ToString()
                                    + "templates"
                                    + Path.DirectorySeparatorChar.ToString()
                                    + "visitor-recorder-registration-approved-email-template.html";

                string[] text;

                using (StreamReader streamReader = System.IO.File.OpenText(pathToFile))
                {
                    text = streamReader.ReadToEnd().Split('@');

                    text[4] = _localization["Layout.Header.Title.Text"].Value;
                    text[6] = _localization["EmailTemplates.RegisterApplicationApproved.Text1"].Value;
                    text[8] = user.Name + " " + user.Surname;
                    text[10] = _localization["EmailTemplates.RegisterApplicationApproved.Text2"].Value;
                    text[12] = _localization["EmailTemplates.RegisterApplicationApproved.Text3"].Value;
                    text[14] = _localization["EmailTemplates.RegisterApplicationApproved.Text4"].Value;
                    text[16] = user.Email;
                    text[18] = _localization["EmailTemplates.RegisterApplicationApproved.Text5"].Value;
                    text[20] = "12345";
                    text[22] = _localization["EmailTemplates.RegisterApplicationApproved.Text6"].Value;
                    text[24] = _localization["EmailTemplates.RegisterApplicationApproved.Text7"].Value;
                    text[26] = _localization["EmailTemplates.RegisterApplicationApproved.Text8"].Value;
                    text[28] = _localization["EmailTemplates.RegisterApplicationApproved.Text8"].Value;
                    text[30] = _localization["EmailTemplates.RegisterApplicationApproved.Text9"].Value;
                    text[32] = _localization["EmailTemplates.RegisterApplicationApproved.Text10"].Value;
                    text[34] = _localization["EmailTemplates.RegisterApplicationApproved.Text10"].Value;
                    text[36] = _localization["EmailTemplates.RegisterApplicationApproved.Text11"].Value;
                }

                await _emailService.SendEmail(user.Email, _localization["EmailTemplates.RegisterApplication.Text1"].Value, String.Concat(text));
            }

            else if (registerApplicationViewModel.RegisterApplication.Status == RegisterApplicationStatus.Rejected.ToString())
            {
                var pathToFile = _environment.WebRootPath
                                    + Path.DirectorySeparatorChar.ToString()
                                    + "templates"
                                    + Path.DirectorySeparatorChar.ToString()
                                    + "visitor-recorder-registration-rejected-email-template.html";

                string[] text;

                using (StreamReader streamReader = System.IO.File.OpenText(pathToFile))
                {
                    text = streamReader.ReadToEnd().Split('@');

                    text[4] = _localization["Layout.Header.Title.Text"].Value;
                    text[6] = _localization["EmailTemplates.RegisterApplicationRejected.Text1"].Value;
                    text[8] = user.Name + " " + user.Surname;
                    text[10] = _localization["EmailTemplates.RegisterApplicationRejected.Text2"].Value;
                    text[12] = _localization["EmailTemplates.RegisterApplicationRejected.Text3"].Value;
                    text[14] = _localization["EmailTemplates.RegisterApplicationRejected.Text4"].Value;
                    text[16] = _localization["EmailTemplates.RegisterApplicationRejected.Text4"].Value;
                    text[18] = _localization["EmailTemplates.RegisterApplicationRejected.Text5"].Value;
                    text[20] = _localization["EmailTemplates.RegisterApplicationRejected.Text6"].Value;
                    text[22] = _localization["EmailTemplates.RegisterApplicationRejected.Text6"].Value;
                    text[24] = _localization["EmailTemplates.RegisterApplicationRejected.Text7"].Value;
                }

                await _emailService.SendEmail(user.Email, _localization["EmailTemplates.RegisterApplication.Text1"].Value, String.Concat(text));
            }

            return Json(new { isValid = true, message = _localization["RegisterApplications.Notification.Edit.Text"].Value });
        }
    }
}
