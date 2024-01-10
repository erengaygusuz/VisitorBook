using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Dtos.UserDataStatisticDtos;
using VisitorBook.Core.ViewModels;
using VisitorBook.UI.Languages;

namespace VisitorBook.UI.Area.App.Controllers
{
    [Authorize]
    [Area("App")]
    public class HomeController : Controller
    {
        private readonly IPlaceStatisticService _placeStatisticService;
        private readonly IUserDataStatisticService _userDataStatisticService;
        private readonly IUserStatisticService _userStatisticService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<Language> _localization;

        public HomeController(IUserDataStatisticService userDataStatisticService, IMapper mapper, IStringLocalizer<Language> localization,
             IUserStatisticService userStatisticService, IPlaceStatisticService placeStatisticService) 
        { 
            _userDataStatisticService = userDataStatisticService;
            _mapper = mapper;
            _localization = localization;
            _userStatisticService = userStatisticService;
            _placeStatisticService = placeStatisticService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            VisitorStatisticViewModel visitorStatisticViewModel = new VisitorStatisticViewModel()
            {
                GetHighestCountOfVisitedCountryByVisitor = _mapper.Map<HighestCountOfVisitedCountryByVisitorResponseDto>(
                    await _userDataStatisticService.GetHighestCountOfVisitedCountryByVisitorAsync()),

                GetHighestCountOfVisitedCityByVisitor = _mapper.Map<HighestCountOfVisitedCityByVisitorResponseDto>(
                    await _userDataStatisticService.GetHighestCountOfVisitedCityByVisitorAsync()),

                GetHighestCountOfVisitedCountyByVisitor = _mapper.Map<HighestCountOfVisitedCountyByVisitorResponseDto>(
                    await _userDataStatisticService.GetHighestCountOfVisitedCountyByVisitorAsync()),

                GetLongestDistanceByVisitorOneTime = _mapper.Map<LongestDistanceByVisitorOneTimeResponseDto>(
                    await _userDataStatisticService.GetLongestDistanceByVisitorOneTimeAsync()),

                GetLongestDistanceByVisitorAllTime = _mapper.Map<LongestDistanceByVisitorAllTimeResponseDto>(
                    await _userDataStatisticService.GetLongestDistanceByVisitorAllTimeAsync()),

                GetTotalUserCount = _userStatisticService.GetTotalUserCount(),

                GetTotalVisitorUserCount = await _userStatisticService.GetTotalVisitorUserCountAsync(),

                GetTotalVisitorRecorderUserCount = await _userStatisticService.GetTotalVisitorRecorderUserCountAsync(),

                GetTotalAdminUserCount = await _userStatisticService.GetTotalAdminUserCountAsync(),

                GetTotalRegionCount = _placeStatisticService.GetTotalRegionCount(),

                GetTotalSubRegionCount = _placeStatisticService.GetTotalSubRegionCount(),

                GetTotalCountryCount = _placeStatisticService.GetTotalCountryCount(),

                GetTotalCityCount = _placeStatisticService.GetTotalCityCount(),

                GetTotalCountyCount = _placeStatisticService.GetTotalCountyCount(),

                GetTotalVisitedCountyCount = _placeStatisticService.GetTotalVisitedCountyCount()
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

        [HttpGet]
        public IActionResult AccessDenied(string returnUrl)
        {
            ViewBag.message = _localization["AccessDenied.Message.Text"].Value;

            return View();
        }
    }
}