using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Models;
using VisitorBook.Core.Utilities.DataTablesServerSideHelpers;
using VisitorBook.Core.Extensions;
using VisitorBook.Core.Dtos.CountyDtos;
using Microsoft.EntityFrameworkCore;

namespace VisitorBook.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountiesController : BaseController
    {
        private readonly IService<County> _countyService;

        public CountiesController(IService<County> countyService, IMapper mapper) : base(mapper)
        {
            _countyService = countyService;
        }

        [HttpPost]
        [Route("GetTableData")]
        public IActionResult GetAllCounties([FromBody] DataTablesOptions model)
        {
            return DataTablesResult(_countyService.GetAll<CountyGetResponseDto>(model, include: x => x.Include(c => c.City)));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrderedCounties()
        {
            var counties = await _countyService.GetAllAsync(orderBy: o => o.OrderBy(x => x.Name));

            if (counties == null)
            {
                return NotFound();
            }

            var countyGetResponseDtos = _mapper.Map<List<CountyGetResponseDto>>(counties);

            return Ok(countyGetResponseDtos);
        }

        [HttpGet]
        [Route("GetAllCountiesByCity/{cityId}")]
        public async Task<IActionResult> GetAllCountiesByCity(Guid cityId)
        {
            var counties = await _countyService.GetAllAsync(orderBy: o => o.OrderBy(x => x.Name), expression: u => u.CityId == cityId);

            if (counties == null)
            {
                return NotFound();
            }

            var countyGetResponseDtos = _mapper.Map<List<CountyGetResponseDto>>(counties);

            return Ok(countyGetResponseDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCounty(Guid id)
        {
            var county = await _countyService.GetAsync(u => u.Id == id);

            if (county == null)
            {
                return NotFound();
            }

            var countyGetResponseDto = _mapper.Map<CountyGetResponseDto>(county);

            return Ok(countyGetResponseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCounty(Guid id)
        {
            var countyToDelete = await _countyService.GetAsync(u => u.Id == id);

            if (countyToDelete == null)
            {
                return NotFound();
            }

            await _countyService.RemoveAsync(countyToDelete);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCounty([FromBody] CountyAddRequestDto countyAddRequestDto)
        {
            if (countyAddRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            var countyToAdd = _mapper.Map<County>(countyAddRequestDto);

            await _countyService.AddAsync(countyToAdd);

            return CreatedAtRoute("GetCounty", countyToAdd, new { id = countyToAdd.Id });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCounty([FromBody] CountyUpdateRequestDto countyUpdateRequestDto)
        {
            if (countyUpdateRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            var countyToUpdate = _mapper.Map<County>(countyUpdateRequestDto);

            if (countyToUpdate == null)
            {
                return NotFound();
            }

            await _countyService.UpdateAsync(countyToUpdate);

            return NoContent();
        }
    }
}
