using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Constants;
using VisitorBook.Core.ViewModels;

namespace VisitorBook.UI.Areas.App.Controllers
{
    [Authorize]
    [Area("App")]
    public class FakeDataController : Controller
    {
        private readonly IFakeDataService _fakeDataService;
        private readonly IValidator<FakeDataViewModel> _fakeDataViewModelValidator;

        public FakeDataController(IFakeDataService fakeDataService, IValidator<FakeDataViewModel> fakeDataViewModelValidator)
        {
            _fakeDataService = fakeDataService;
            _fakeDataViewModelValidator = fakeDataViewModelValidator;
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

            return RedirectToAction("Index");
        }
    }
}
