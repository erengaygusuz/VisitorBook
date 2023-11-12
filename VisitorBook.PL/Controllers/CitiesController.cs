using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Dtos.CityDtos;
using VisitorBook.Core.Models;
using VisitorBook.Core.Utilities.DataTablesServerSideHelpers;
using VisitorBook.Core.Extensions;

namespace VisitorBook.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : BaseController
    {
        private readonly IService<City> _cityService;

        public CitiesController(IService<City> cityService, IMapper mapper) : base(mapper)
        {
            _cityService = cityService;
        }

        [HttpPost]
        [Route("GetTableData")]
        public IActionResult GetAllCities([FromBody] DataTablesOptions model)
        {
            return DataTablesResult(_cityService.GetAll<CityGetResponseDto>(model));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity(Guid id)
        {
            var city = await _cityService.GetAsync(u => u.Id == id);

            if (city == null)
            {
                return NotFound();
            }

            var cityGetResponseDto = _mapper.Map<CityGetResponseDto>(city);

            return Ok(cityGetResponseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(Guid id)
        {
            var cityToDelete = await _cityService.GetAsync(u => u.Id == id);

            if (cityToDelete == null)
            {
                return NotFound();
            }

            await _cityService.RemoveAsync(cityToDelete);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCity([FromBody] CityAddRequestDto cityAddRequestDto)
        {
            if (cityAddRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            var cityToAdd = _mapper.Map<City>(cityAddRequestDto);

            await _cityService.AddAsync(cityToAdd);

            return CreatedAtRoute("GetCity", cityToAdd, new { id = cityToAdd.Id });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCity([FromBody] CityUpdateRequestDto cityUpdateRequestDto)
        {
            if (cityUpdateRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            var cityToUpdate = _mapper.Map<City>(cityUpdateRequestDto);

            if (cityToUpdate == null)
            {
                return NotFound();
            }

            await _cityService.UpdateAsync(cityToUpdate);

            return NoContent();
        }
    }
}
