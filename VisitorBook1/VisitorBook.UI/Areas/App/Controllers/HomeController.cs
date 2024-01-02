using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Dtos.VisitorStatisticDtos;
using VisitorBook.Core.ViewModels;
using VisitorBook.UI.Languages;

namespace VisitorBook.UI.Area.App.Controllers
{
    [Authorize]
    [Area("App")]
    public class HomeController : Controller
    {
        private readonly IVisitorStatisticService _visitorStatisticService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<Language> _localization;

        public HomeController(IVisitorStatisticService visitorStatisticService, IMapper mapper, IStringLocalizer<Language> localization) 
        { 
            _visitorStatisticService = visitorStatisticService;
            _mapper = mapper;
            _localization = localization;
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
            ViewBag.message = _localization["AccessDenied.Message.Text"].Value;

            return View();
        }
    }
}