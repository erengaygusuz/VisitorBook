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

        public FakeDataController(IFakeDataService fakeDataService)
        {
            _fakeDataService = fakeDataService;
        }

        [Authorize(Permissions.FakeDataManagement.View)]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Permissions.FakeDataManagement.Create)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateUser(FakeDataViewModel fakeDataViewModel)
        {
            ModelState.Remove("VisitedCountyAmount");

            if (!ModelState.IsValid)
            {
                return View("Index", fakeDataViewModel);
            }

            await _fakeDataService.InsertUserDatas(fakeDataViewModel.UserAmount);

            return View("Index");
        }

        [Authorize(Permissions.FakeDataManagement.Create)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateVisitedCounty(FakeDataViewModel fakeDataViewModel)
        {
            ModelState.Remove("UserAmount");

            if (!ModelState.IsValid)
            {
                return View("Index", fakeDataViewModel);
            }
            
            await _fakeDataService.InsertVisitedCountyDatas(fakeDataViewModel.VisitedCountyAmount);

            return View("Index");
        }
    }
}
