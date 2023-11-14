
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Dtos.VisitedCountyDtos;
using VisitorBook.Core.Models;
using VisitorBook.Core.Utilities.DataTablesServerSideHelpers;
using VisitorBook.Core.Extensions;

namespace VisitorBook.PL.Controllers
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
        public IActionResult GetAllVisitedCounties([FromBody] DataTablesOptions model)
        {
            return DataTablesResult(_visitedCountyService.GetAll<VisitedCountyGetResponseDto>(model, 
                include: x => x.Include(c => c.Visitor).Include(c => c.County).ThenInclude(c => c.City)));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrderedVisitedCounties()
        {
            var visitedCounties = await _visitedCountyService.GetAllAsync(orderBy: o => o.OrderBy(x => x.Visitor.Name), 
                include: x => x.Include(c => c.Visitor));

            if (visitedCounties == null)
            {
                return NotFound();
            }

            var visitedCountyGetResponseDtos = _mapper.Map<List<VisitedCountyGetResponseDto>>(visitedCounties);

            return Ok(visitedCountyGetResponseDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVisitedCounty(Guid id)
        {
            var visitedCounty = await _visitedCountyService.GetAsync(u => u.Id == id, include: x => x.Include(c => c.County));

            if (visitedCounty == null)
            {
                return NotFound();
            }

            var visitedCountyGetResponseDto = _mapper.Map<VisitedCountyGetResponseDto>(visitedCounty);

            return Ok(visitedCountyGetResponseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVisitedCounty(Guid id)
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
        public async Task<IActionResult> CreateVisitedCounty([FromBody] VisitedCountyAddRequestDto visitedCountyAddRequestDto)
        {
            if (visitedCountyAddRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            var visitedCountyToAdd = _mapper.Map<VisitedCounty>(visitedCountyAddRequestDto);

            await _visitedCountyService.AddAsync(visitedCountyToAdd);

            return CreatedAtRoute("GetVisitedCounty", visitedCountyToAdd, new { id = visitedCountyToAdd.Id });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateVisitedCounty([FromBody] VisitedCountyUpdateRequestDto visitedCountyUpdateRequestDto)
        {
            if (visitedCountyUpdateRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            var visitedCountyToUpdate = _mapper.Map<VisitedCounty>(visitedCountyUpdateRequestDto);

            if (visitedCountyToUpdate == null)
            {
                return NotFound();
            }

            await _visitedCountyService.UpdateAsync(visitedCountyToUpdate);

            return NoContent();
        }
    }
}
