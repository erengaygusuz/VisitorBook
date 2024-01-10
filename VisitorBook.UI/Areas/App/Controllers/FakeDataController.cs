using AspNetCoreHero.ToastNotification.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Constants;
using VisitorBook.Core.ViewModels;
using VisitorBook.UI.Languages;

namespace VisitorBook.UI.Areas.App.Controllers
{
    [Authorize]
    [Area("App")]
    public class FakeDataController : Controller
    {
        private readonly IFakeDataService _fakeDataService;
        private readonly IValidator<FakeDataViewModel> _fakeDataViewModelValidator;
        private readonly INotyfService _notifyService;
        private readonly IStringLocalizer<Language> _localization;

        public FakeDataController(IFakeDataService fakeDataService, IValidator<FakeDataViewModel> fakeDataViewModelValidator, INotyfService notifyService,
            IStringLocalizer<Language> localization)
        {
            _fakeDataService = fakeDataService;
            _fakeDataViewModelValidator = fakeDataViewModelValidator;
            _notifyService = notifyService;
            _localization = localization;
        }

        [Authorize(Permissions.FakeDataManagement.View)]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Permissions.FakeDataManagement.Create)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateUser(FakeDataViewModel fakeDataViewModel)
        {
            var validationResult = await _fakeDataViewModelValidator.ValidateAsync(fakeDataViewModel, options =>
            {
                options.IncludeProperties("UserAmount");
            });

            if (!validationResult.IsValid)
            {
                TempData["ErrorMessage1"] = validationResult.Errors.FirstOrDefault().ErrorMessage;

                return RedirectToAction("Index");
            }

            await _fakeDataService.InsertUserDatas(fakeDataViewModel.UserAmount);

            _notifyService.Success(_localization["FakeDatas.Notification.UserAdd.Text"].Value);

            return RedirectToAction("Index");
        }

        [Authorize(Permissions.FakeDataManagement.Create)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateVisitedCounty(FakeDataViewModel fakeDataViewModel)
        {
            var validationResult = await _fakeDataViewModelValidator.ValidateAsync(fakeDataViewModel, options =>
            {
                options.IncludeProperties("VisitedCountyAmount");
            });

            if (!validationResult.IsValid)
            {
                TempData["ErrorMessage2"] = validationResult.Errors.FirstOrDefault().ErrorMessage;

                return RedirectToAction("Index");
            }
            
            await _fakeDataService.InsertVisitedCountyDatas(fakeDataViewModel.VisitedCountyAmount);

            _notifyService.Success(_localization["FakeDatas.Notification.VisitedCountyAdd.Text"].Value);

            return RedirectToAction("Index");
        }
    }
}
