using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VisitorBook.Backend.BL.Concrete;
using VisitorBook.Backend.Core.Dtos.VisitorStatisticDtos;

namespace VisitorBook.Backend.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitorStatisticsController : ControllerBase
    {
        private readonly VisitorStatisticService _visitorStatisticService;
        private readonly IMapper _mapper;

        public VisitorStatisticsController(VisitorStatisticService visitorStatisticService, IMapper mapper)
        {
            _visitorStatisticService = visitorStatisticService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetHighestCountOfVisitedCountyByVisitor")]
        public async Task<IActionResult> GetHighestCountOfVisitedCountyByVisitor()
        {
            var highestCountOfVisitedCountyByVisitor = await _visitorStatisticService.GetHighestCountOfVisitedCountyByVisitorAsync();

            if (highestCountOfVisitedCountyByVisitor == null)
            {
                return NotFound();
            }

            var highestCountOfVisitedCountyByVisitorResponseDto = _mapper.Map<HighestCountOfVisitedCountyByVisitorResponseDto>(highestCountOfVisitedCountyByVisitor);

            return Ok(highestCountOfVisitedCountyByVisitorResponseDto);
        }

        [HttpGet]
        [Route("GetHighestCountOfVisitedCityByVisitor")]
        public async Task<IActionResult> GetHighestCountOfVisitedCityByVisitor()
        {
            var highestCountOfVisitedCityByVisitor = await _visitorStatisticService.GetHighestCountOfVisitedCityByVisitorAsync();

            if (highestCountOfVisitedCityByVisitor == null)
            {
                return NotFound();
            }

            var highestCountOfVisitedCityByVisitorResponseDto = _mapper.Map<HighestCountOfVisitedCityByVisitorResponseDto>(highestCountOfVisitedCityByVisitor);

            return Ok(highestCountOfVisitedCityByVisitorResponseDto);
        }

        [HttpGet]
        [Route("GetLongestDistanceByVisitorOneTime")]
        public async Task<IActionResult> GetLongestDistanceByVisitorOneTime()
        {
            var longestDistanceByVisitorOneTime = await _visitorStatisticService.GetLongestDistanceByVisitorOneTimeAsync();

            if (longestDistanceByVisitorOneTime == null)
            {
                return NotFound();
            }

            var longestDistanceByVisitorOneTimeResponseDto = _mapper.Map<LongestDistanceByVisitorOneTimeResponseDto>(longestDistanceByVisitorOneTime);

            return Ok(longestDistanceByVisitorOneTimeResponseDto);
        }

        [HttpGet]
        [Route("GetLongestDistanceByVisitorAllTime")]
        public async Task<IActionResult> GetLongestDistanceByVisitorAllTime()
        {
            var longestDistanceByVisitorAllTime = await _visitorStatisticService.GetLongestDistanceByVisitorAllTimeAsync();

            if (longestDistanceByVisitorAllTime == null)
            {
                return NotFound();
            }

            var longestDistanceByVisitorAllTimeResponseDto = _mapper.Map<LongestDistanceByVisitorAllTimeResponseDto>(longestDistanceByVisitorAllTime);

            return Ok(longestDistanceByVisitorAllTimeResponseDto);
        }
    }
}
