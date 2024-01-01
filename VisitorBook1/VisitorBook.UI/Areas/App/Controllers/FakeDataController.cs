using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.ViewModels;

namespace VisitorBook.UI.Areas.App.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("App")]
    public class FakeDataController : Controller
    {
        private readonly IFakeDataService _fakeDataService;

        public FakeDataController(IFakeDataService fakeDataService)
        {
            _fakeDataService = fakeDataService;
        }

        public IActionResult Index()
        {
            return View();
        }

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
