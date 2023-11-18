using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Dtos.CityDtos;
using VisitorBook.Core.Entities;
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
        public IActionResult GetAllCities([FromBody] DataTablesOptions dataTablesOptions)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            return DataTablesResult(_cityService.GetAll<CityResponseDto>(dataTablesOptions));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrderedCities()
        {
            var cities = await _cityService.GetAllAsync(orderBy: o => o.OrderBy(x => x.Name));

            if (cities == null)
            {
                return NotFound();
            }

            var cityResponseDtos = _mapper.Map<List<CityResponseDto>>(cities);

            return Ok(cityResponseDtos);
        }

        [HttpGet("{id}", Name = "GetCity")]
        public async Task<IActionResult> GetCity(Guid id)
        {
            var city = await _cityService.GetAsync(u => u.Id == id);

            if (city == null)
            {
                return NotFound();
            }

            var cityResponseDto = _mapper.Map<CityResponseDto>(city);

            return Ok(cityResponseDto);
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
        public async Task<IActionResult> CreateCity([FromBody] CityRequestDto cityRequestDto)
        {
            if (cityRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            var cityToAdd = _mapper.Map<City>(cityRequestDto);

            await _cityService.AddAsync(cityToAdd);

            return CreatedAtRoute("GetCity", cityToAdd, new { id = cityToAdd.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCity(Guid id, [FromBody] CityRequestDto cityRequestDto)
        {
            var cityToUpdate = await _cityService.GetAsync(c => c.Id == id);

            if (cityToUpdate == null)
            {
                return NotFound();
            }

            if (cityRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            _mapper.Map(cityRequestDto, cityToUpdate);

            await _cityService.UpdateAsync(cityToUpdate);

            return NoContent();
        }
    }
}
