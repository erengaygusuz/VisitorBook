using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisitorBook.Backend.Core.Abstract;
using VisitorBook.Backend.Core.Entities;
using VisitorBook.Backend.Core.Utilities.DataTablesServerSideHelpers;
using VisitorBook.Backend.Core.Extensions;
using VisitorBook.Backend.Core.Dtos.VisitorDtos;

namespace VisitorBook.Backend.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitorsController : BaseController
    {
        private readonly IService<Visitor> _visitorService;

        public VisitorsController(IService<Visitor> visitorService, IMapper mapper) : base(mapper)
        {
            _visitorService = visitorService;
        }

        [HttpPost]
        [Route("GetTableData")]
        public IActionResult GetAllVisitors([FromBody] DataTablesOptions dataTablesOptions)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            return DataTablesResult(_visitorService.GetAll<VisitorResponseDto>(dataTablesOptions));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVisitors()
        {
            var visitors = await _visitorService.GetAllAsync(
                orderBy: o => o.OrderBy(x => x.User.Name),
                include: x => x.Include(c => c.VisitorAddress).ThenInclude(c => c.County)
                );

            if (visitors == null)
            {
                return NotFound();
            }

            var visitorResponseDtos = _mapper.Map<List<VisitorResponseDto>>(visitors);

            return Ok(visitorResponseDtos);
        }

        [HttpGet("{id}", Name = "GetVisitor")]
        public async Task<IActionResult> GetVisitor(Guid id)
        {
            var visitor = await _visitorService.GetAsync(u => u.GId == id, 
                include: x => x.Include(c => c.VisitorAddress).ThenInclude(c => c.County));

            if (visitor == null)
            {
                return NotFound();
            }

            var visitorResponseDto = _mapper.Map<VisitorResponseDto>(visitor);

            return Ok(visitorResponseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVisitor(Guid id)
        {
            var visitorToDelete = await _visitorService.GetAsync(u => u.GId == id);

            if (visitorToDelete == null)
            {
                return NotFound();
            }

            await _visitorService.RemoveAsync(visitorToDelete);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateVisitor([FromBody] VisitorRequestDto visitorRequestDto)
        {
            if (visitorRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            var visitorToAdd = _mapper.Map<Visitor>(visitorRequestDto);

            await _visitorService.AddAsync(visitorToAdd);

            return CreatedAtRoute("GetVisitedCounty", visitorToAdd, new { id = visitorToAdd.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVisitor(Guid id, [FromBody] VisitorRequestDto visitorRequestDto)
        {
            var visitorToUpdate = await _visitorService.GetAsync(c => c.GId == id);

            if (visitorToUpdate == null)
            {
                return NotFound();
            }

            if (visitorRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            _mapper.Map(visitorRequestDto, visitorToUpdate);

            await _visitorService.UpdateAsync(visitorToUpdate);

            return NoContent();
        }
    }
}
