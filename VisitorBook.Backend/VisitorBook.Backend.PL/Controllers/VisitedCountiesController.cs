using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisitorBook.Backend.Core.Abstract;
using VisitorBook.Backend.Core.Dtos.VisitedCountyDtos;
using VisitorBook.Backend.Core.Entities;
using VisitorBook.Backend.Core.Utilities.DataTablesServerSideHelpers;
using VisitorBook.Backend.Core.Extensions;

namespace VisitorBook.Backend.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitedCountiesController : BaseController
    {
        private readonly IService<VisitedCounty> _visitedCountyService;

        public VisitedCountiesController(IService<VisitedCounty> visitedCountyService, IMapper mapper) : base(mapper)
        {
            _visitedCountyService = visitedCountyService;
        }

        [HttpPost]
        [Route("GetTableData")]
        public IActionResult GetAllVisitedCounties([FromBody] DataTablesOptions dataTablesOptions)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            return DataTablesResult(_visitedCountyService.GetAll<VisitedCountyResponseDto>(dataTablesOptions, 
                include: x => x.Include(c => c.Visitor).Include(c => c.County).ThenInclude(c => c.City)));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVisitedCounties()
        {
            var visitedCounties = await _visitedCountyService.GetAllAsync(orderBy: o => o.OrderBy(x => x.Visitor.User.Name),
                include: x => x.Include(c => c.Visitor).Include(c => c.County).ThenInclude(c => c.City));

            if (visitedCounties == null)
            {
                return NotFound();
            }

            var visitedCountyResponseDtos = _mapper.Map<List<VisitedCountyResponseDto>>(visitedCounties);

            return Ok(visitedCountyResponseDtos);
        }

        [HttpGet("{id}", Name = "GetVisitedCounty")]
        public async Task<IActionResult> GetVisitedCounty(int id)
        {
            var visitedCounty = await _visitedCountyService.GetAsync(u => u.Id == id,
                include: x => x.Include(c => c.Visitor).Include(c => c.County).ThenInclude(c => c.City));

            if (visitedCounty == null)
            {
                return NotFound();
            }

            var visitedCountyResponseDto = _mapper.Map<VisitedCountyResponseDto>(visitedCounty);

            return Ok(visitedCountyResponseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVisitedCounty(int id)
        {
            var visitedCountyToDelete = await _visitedCountyService.GetAsync(u => u.Id == id);

            if (visitedCountyToDelete == null)
            {
                return NotFound();
            }

            await _visitedCountyService.RemoveAsync(visitedCountyToDelete);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateVisitedCounty([FromBody] VisitedCountyRequestDto visitedCountyRequestDto)
        {
            if (visitedCountyRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            var visitedCountyToAdd = _mapper.Map<VisitedCounty>(visitedCountyRequestDto);

            await _visitedCountyService.AddAsync(visitedCountyToAdd);

            return CreatedAtRoute("GetVisitedCounty", visitedCountyToAdd, new { id = visitedCountyToAdd.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVisitedCounty(int id, [FromBody] VisitedCountyRequestDto visitedCountyRequestDto)
        {
            var visitedCountyToUpdate = await _visitedCountyService.GetAsync(c => c.Id == id);

            if (visitedCountyToUpdate == null)
            {
                return NotFound();
            }

            if (visitedCountyRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            _mapper.Map(visitedCountyRequestDto, visitedCountyToUpdate);

            await _visitedCountyService.UpdateAsync(visitedCountyToUpdate);

            return NoContent();
        }
    }
}
