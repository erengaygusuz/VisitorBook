using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VisitorBook.Backend.Core.Abstract;
using VisitorBook.Backend.Core.Dtos.RegionDtos;
using VisitorBook.Backend.Core.Entities;
using VisitorBook.Backend.Core.Utilities.DataTablesServerSideHelpers;
using VisitorBook.Backend.Core.Extensions;

namespace VisitorBook.Backend.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : BaseController
    {
        private readonly IService<Region> _regionService;

        public RegionsController(IService<Region> regionService, IMapper mapper) : base(mapper)
        {
            _regionService = regionService;
        }

        [HttpPost]
        [Route("GetTableData")]
        public IActionResult GetAllRegions([FromBody] DataTablesOptions dataTablesOptions)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            return DataTablesResult(_regionService.GetAll<RegionResponseDto>(dataTablesOptions));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            var regions = await _regionService.GetAllAsync(orderBy: o => o.OrderBy(x => x.Name));

            if (regions == null)
            {
                return NotFound();
            }

            var regionResponseDtos = _mapper.Map<List<RegionResponseDto>>(regions);

            return Ok(regionResponseDtos);
        }

        [HttpGet("{id}", Name = "GetRegion")]
        public async Task<IActionResult> GetRegion(int id)
        {
            var region = await _regionService.GetAsync(u => u.Id == id);

            if (region == null)
            {
                return NotFound();
            }

            var regionResponseDto = _mapper.Map<RegionResponseDto>(region);

            return Ok(regionResponseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegion(int id)
        {
            var regionToDelete = await _regionService.GetAsync(u => u.Id == id);

            if (regionToDelete == null)
            {
                return NotFound();
            }

            await _regionService.RemoveAsync(regionToDelete);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRegion([FromBody] RegionRequestDto regionRequestDto)
        {
            if (regionRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            var regionToAdd = _mapper.Map<Region>(regionRequestDto);

            await _regionService.AddAsync(regionToAdd);

            return CreatedAtRoute("GetRegion", regionToAdd, new { id = regionToAdd.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRegion(int id, [FromBody] RegionRequestDto regionRequestDto)
        {
            var regionToUpdate = await _regionService.GetAsync(c => c.Id == id);

            if (regionToUpdate == null)
            {
                return NotFound();
            }

            if (regionRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            _mapper.Map(regionRequestDto, regionToUpdate);

            await _regionService.UpdateAsync(regionToUpdate);

            return NoContent();
        }
    }
}
