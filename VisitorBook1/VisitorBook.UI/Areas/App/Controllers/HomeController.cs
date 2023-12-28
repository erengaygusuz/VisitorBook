using AutoMapper;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using VisitorBook.BL.Concrete;
using VisitorBook.Core.Dtos.VisitorStatisticDtos;
using VisitorBook.UI.ViewModels;

namespace VisitorBook.UI.Area.App.Controllers
{
    [Area("App")]
    public class HomeController : Controller
    {
        private readonly VisitorStatisticService _visitorStatisticService;
        private readonly IMapper _mapper;

        public HomeController(VisitorStatisticService visitorStatisticService, IMapper mapper) 
        { 
            _visitorStatisticService = visitorStatisticService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            VisitorStatisticViewModel visitorStatisticViewModel = new VisitorStatisticViewModel()
            {
                GetHighestCountOfVisitedCityByVisitor = _mapper.Map<HighestCountOfVisitedCityByVisitorResponseDto>(await _visitorStatisticService.GetHighestCountOfVisitedCityByVisitorAsync()),
                GetHighestCountOfVisitedCountyByVisitor = _mapper.Map<HighestCountOfVisitedCountyByVisitorResponseDto>(await _visitorStatisticService.GetHighestCountOfVisitedCountyByVisitorAsync()),
                GetLongestDistanceByVisitorOneTime = _mapper.Map<LongestDistanceByVisitorOneTimeResponseDto>(await _visitorStatisticService.GetLongestDistanceByVisitorOneTimeAsync()),
                GetLongestDistanceByVisitorAllTime = _mapper.Map<LongestDistanceByVisitorAllTimeResponseDto>(await _visitorStatisticService.GetLongestDistanceByVisitorAllTimeAsync())
            };            

            return View(visitorStatisticViewModel);
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

        public IActionResult AccessDenied(string returnUrl)
        {
            string message = string.Empty;

            message = "Bu sayfayı görmeye yetkiniz yoktur. Yetki almak için yöneticiniz ile görüşünüz.";

            ViewBag.message = message;

            return View();
        }
    }
}