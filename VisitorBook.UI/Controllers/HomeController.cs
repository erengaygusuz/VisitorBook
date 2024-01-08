using AspNetCoreHero.ToastNotification.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Dtos.ContactMessageDtos;
using VisitorBook.Core.Entities;
using VisitorBook.Core.ViewModels;
using VisitorBook.UI.Languages;

namespace VisitorBook.UI.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IHomeFactStatisticService _homeFactStatisticService;
        private readonly IService<ContactMessage> _contactMessageService;
        private readonly IValidator<ContactMessageRequestDto> _contactMessageRequestDtoValidator;
        private readonly INotyfService _notifyService;
        private readonly IStringLocalizer<Language> _localization;

        public HomeController(IHomeFactStatisticService homeFactStatisticService, IService<ContactMessage> contactMessageService, 
            IValidator<ContactMessageRequestDto> contactMessageRequestDtoValidator, INotyfService notifyService, IStringLocalizer<Language> localization)
        {
            _homeFactStatisticService = homeFactStatisticService;
            _contactMessageService = contactMessageService;
            _contactMessageRequestDtoValidator = contactMessageRequestDtoValidator;
            _notifyService = notifyService;
            _localization = localization;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var countryCount = _homeFactStatisticService.GetTotalCountryCount();

            var visitedLocationCount = _homeFactStatisticService.GetTotalVisitedLocationCount();

            var visitorCount = await _homeFactStatisticService.GetTotalVisitorCountAsync();

            var userCount = _homeFactStatisticService.GetTotalUserCount();

            var homeFactViewModel = new HomeFactViewModel
            {
                CountryCount = countryCount,
                VisitedLocationCount = visitedLocationCount,
                VisitorCount = visitorCount,
                UserCount = userCount
            };

            return View(homeFactViewModel);
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactMessageRequestDto contactMessageRequestDto)
        {
            var validationResult = await _contactMessageRequestDtoValidator.ValidateAsync(contactMessageRequestDto);

            if (!validationResult.IsValid)
            {
                return View(contactMessageRequestDto);
            }

            await _contactMessageService.AddAsync(contactMessageRequestDto);

            _notifyService.Success(_localization["ContactMessages.Notification.SuccessfullSend.Text"].Value);

            return View();
        }

        [HttpGet]
        public IActionResult Service()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SetCulture(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(30) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}