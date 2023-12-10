﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VisitorBook.Backend.Core.Abstract;
using VisitorBook.Backend.Core.Entities;
using VisitorBook.Backend.Core.Utilities.DataTablesServerSideHelpers;
using VisitorBook.Backend.Core.Extensions;
using VisitorBook.Backend.Core.Dtos.CountyDtos;
using Microsoft.EntityFrameworkCore;

namespace VisitorBook.Backend.PL.Controllers
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
        public IActionResult GetAllCounties([FromBody] DataTablesOptions dataTablesOptions)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            return DataTablesResult(_countyService.GetAll<CountyResponseDto>(dataTablesOptions, include: x => x.Include(c => c.City)));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCounties()
        {
            var counties = await _countyService.GetAllAsync(orderBy: o => o.OrderBy(x => x.Name), include: x => x.Include(c => c.City));

            if (counties == null)
            {
                return NotFound();
            }

            var countyResponseDtos = _mapper.Map<List<CountyResponseDto>>(counties);

            return Ok(countyResponseDtos);
        }

        [HttpGet]
        [Route("GetAllCountiesByCity/{cityId}")]
        public async Task<IActionResult> GetAllCountiesByCity(int cityId)
        {
            var counties = await _countyService.GetAllAsync(
                orderBy: o => o.OrderBy(x => x.Name), 
                expression: u => u.CityId == cityId,
                include: x => x.Include(c => c.City));

            if (counties == null)
            {
                return NotFound();
            }

            var countyResponseDtos = _mapper.Map<List<CountyResponseDto>>(counties);

            return Ok(countyResponseDtos);
        }

        [HttpGet("{id}", Name = "GetCounty")]
        public async Task<IActionResult> GetCounty(Guid id)
        {
            var county = await _countyService.GetAsync(u => u.GId == id, include: u => u.Include(a => a.City));

            if (county == null)
            {
                return NotFound();
            }

            var countyResponseDto = _mapper.Map<CountyResponseDto>(county);

            return Ok(countyResponseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCounty(Guid id)
        {
            var countyToDelete = await _countyService.GetAsync(u => u.GId == id);

            if (countyToDelete == null)
            {
                return NotFound();
            }

            await _countyService.RemoveAsync(countyToDelete);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCounty([FromBody] CountyRequestDto countyRequestDto)
        {
            if (countyRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            var countyToAdd = _mapper.Map<County>(countyRequestDto);

            await _countyService.AddAsync(countyToAdd);

            return CreatedAtRoute("GetCounty", countyToAdd, new { id = countyToAdd.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCounty(Guid id, [FromBody] CountyRequestDto countyRequestDto)
        {
            var countyToUpdate = await _countyService.GetAsync(c => c.GId == id);

            if (countyToUpdate == null)
            {
                return NotFound();
            }

            if (countyRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            _mapper.Map(countyRequestDto, countyToUpdate);

            await _countyService.UpdateAsync(countyToUpdate);

            return NoContent();
        }
    }
}
