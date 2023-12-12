﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VisitorBook.Backend.Core.Abstract;
using VisitorBook.Backend.Core.Dtos.SubRegionDtos;
using VisitorBook.Backend.Core.Entities;
using VisitorBook.Backend.Core.Utilities.DataTablesServerSideHelpers;
using VisitorBook.Backend.Core.Extensions;

namespace VisitorBook.Backend.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubRegionsController : BaseController
    {
        private readonly IService<SubRegion> _subRegionService;

        public SubRegionsController(IService<SubRegion> subRegionService, IMapper mapper) : base(mapper)
        {
            _subRegionService = subRegionService;
        }

        [HttpPost]
        [Route("GetTableData")]
        public IActionResult GetAllSubRegions([FromBody] DataTablesOptions dataTablesOptions)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            return DataTablesResult(_subRegionService.GetAll<SubRegionResponseDto>(dataTablesOptions));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubRegions()
        {
            var subRegions = await _subRegionService.GetAllAsync(orderBy: o => o.OrderBy(x => x.Name));

            if (subRegions == null)
            {
                return NotFound();
            }

            var subRegionResponseDtos = _mapper.Map<List<SubRegionResponseDto>>(subRegions);

            return Ok(subRegionResponseDtos);
        }

        [HttpGet("{id}", Name = "GetSubRegion")]
        public async Task<IActionResult> GetSubRegion(Guid id)
        {
            var subRegion = await _subRegionService.GetAsync(u => u.GId == id);

            if (subRegion == null)
            {
                return NotFound();
            }

            var subRegionResponseDto = _mapper.Map<SubRegionResponseDto>(subRegion);

            return Ok(subRegionResponseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubRegion(Guid id)
        {
            var subRegionToDelete = await _subRegionService.GetAsync(u => u.GId == id);

            if (subRegionToDelete == null)
            {
                return NotFound();
            }

            await _subRegionService.RemoveAsync(subRegionToDelete);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubRegion([FromBody] SubRegionRequestDto subRegionRequestDto)
        {
            if (subRegionRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            var subRegionToAdd = _mapper.Map<SubRegion>(subRegionRequestDto);

            await _subRegionService.AddAsync(subRegionToAdd);

            return CreatedAtRoute("GetSubRegion", subRegionToAdd, new { id = subRegionToAdd.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubRegion(Guid id, [FromBody] SubRegionRequestDto subRegionRequestDto)
        {
            var subRegionToUpdate = await _subRegionService.GetAsync(c => c.GId == id);

            if (subRegionToUpdate == null)
            {
                return NotFound();
            }

            if (subRegionRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            _mapper.Map(subRegionRequestDto, subRegionToUpdate);

            await _subRegionService.UpdateAsync(subRegionToUpdate);

            return NoContent();
        }
    }
}
