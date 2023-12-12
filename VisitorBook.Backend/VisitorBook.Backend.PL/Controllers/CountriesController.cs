using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VisitorBook.Backend.Core.Abstract;
using VisitorBook.Backend.Core.Dtos.CountryDtos;
using VisitorBook.Backend.Core.Entities;
using VisitorBook.Backend.Core.Utilities.DataTablesServerSideHelpers;
using VisitorBook.Backend.Core.Extensions;

namespace VisitorBook.Backend.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : BaseController
    {
        private readonly IService<Country> _countryService;

        public CountriesController(IService<Country> countryService, IMapper mapper) : base(mapper)
        {
            _countryService = countryService;
        }

        [HttpPost]
        [Route("GetTableData")]
        public IActionResult GetAllCountries([FromBody] DataTablesOptions dataTablesOptions)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            return DataTablesResult(_countryService.GetAll<CountryResponseDto>(dataTablesOptions));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCountries()
        {
            var countries = await _countryService.GetAllAsync(orderBy: o => o.OrderBy(x => x.Name));

            if (countries == null)
            {
                return NotFound();
            }

            var countryResponseDtos = _mapper.Map<List<CountryResponseDto>>(countries);

            return Ok(countryResponseDtos);
        }

        [HttpGet("{id}", Name = "GetCountry")]
        public async Task<IActionResult> GetCountry(Guid id)
        {
            var country = await _countryService.GetAsync(u => u.GId == id);

            if (country == null)
            {
                return NotFound();
            }

            var countryResponseDto = _mapper.Map<CountryResponseDto>(country);

            return Ok(countryResponseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(Guid id)
        {
            var countryToDelete = await _countryService.GetAsync(u => u.GId == id);

            if (countryToDelete == null)
            {
                return NotFound();
            }

            await _countryService.RemoveAsync(countryToDelete);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCountry([FromBody] CountryRequestDto countryRequestDto)
        {
            if (countryRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            var countryToAdd = _mapper.Map<Country>(countryRequestDto);

            await _countryService.AddAsync(countryToAdd);

            return CreatedAtRoute("GetCountry", countryToAdd, new { id = countryToAdd.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCountry(Guid id, [FromBody] CountryRequestDto countryRequestDto)
        {
            var countryToUpdate = await _countryService.GetAsync(c => c.GId == id);

            if (countryToUpdate == null)
            {
                return NotFound();
            }

            if (countryRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            _mapper.Map(countryRequestDto, countryToUpdate);

            await _countryService.UpdateAsync(countryToUpdate);

            return NoContent();
        }
    }
}
