
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Models;
using VisitorBook.Core.Utilities.DataTablesServerSideHelpers;
using VisitorBook.Core.Extensions;
using VisitorBook.Core.Dtos.VisitorDtos;

namespace VisitorBook.PL.Controllers
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
        public IActionResult GetAllVisitors([FromBody] DataTablesOptions model)
        {
            return DataTablesResult(_visitorService.GetAll<VisitorGetResponseDto>(model));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrderedVisitors()
        {
            var visitors = await _visitorService.GetAllAsync(orderBy: o => o.OrderBy(x => x.Name),
                include: x => x.Include(c => c.VisitorAddress).ThenInclude(c => c.County), expression: v => v.VisitorAddressId != null);

            if (visitors == null)
            {
                return NotFound();
            }

            var visitorGetResponseDtos = _mapper.Map<List<VisitorGetResponseDto>>(visitors);

            return Ok(visitorGetResponseDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVisitor(Guid id)
        {
            var visitor = await _visitorService.GetAsync(u => u.Id == id, 
                include: x => x.Include(c => c.VisitorAddress).ThenInclude(c => c.County));

            if (visitor == null)
            {
                return NotFound();
            }

            var visitorGetResponseDto = _mapper.Map<VisitorGetResponseDto>(visitor);

            return Ok(visitorGetResponseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVisitor(Guid id)
        {
            var visitorToDelete = await _visitorService.GetAsync(u => u.Id == id);

            if (visitorToDelete == null)
            {
                return NotFound();
            }

            await _visitorService.RemoveAsync(visitorToDelete);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateVisitor([FromBody] VisitorAddRequestDto visitorAddRequestDto)
        {
            if (visitorAddRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            var visitorToAdd = _mapper.Map<Visitor>(visitorAddRequestDto);

            await _visitorService.AddAsync(visitorToAdd);

            return CreatedAtRoute("GetVisitedCounty", visitorToAdd, new { id = visitorToAdd.Id });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateVisitor([FromBody] VisitorUpdateRequestDto visitorUpdateRequestDto)
        {
            if (visitorUpdateRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            var visitorToUpdate = _mapper.Map<Visitor>(visitorUpdateRequestDto);

            if (visitorToUpdate == null)
            {
                return NotFound();
            }

            await _visitorService.UpdateAsync(visitorToUpdate);

            return NoContent();
        }
    }
}
