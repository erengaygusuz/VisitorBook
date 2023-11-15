using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Dtos.VisitorAddressDtos;
using VisitorBook.Core.Models;
using VisitorBook.Core.Extensions;

namespace VisitorBook.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitorAddressController : BaseController
    {
        private readonly IService<VisitorAddress> _visitorAddressService;

        public VisitorAddressController(IService<VisitorAddress> visitorAddressService, IMapper mapper) : base(mapper)
        {
            _visitorAddressService = visitorAddressService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrderedVisitorAddresses()
        {
            var visitorAddresses = await _visitorAddressService.GetAllAsync(orderBy: o => o.OrderBy(x => x.County.Name),
                include: x => x.Include(a => a.County));

            if (visitorAddresses == null)
            {
                return NotFound();
            }

            var visitorAddressResponseDtos = _mapper.Map<List<VisitorAddressResponseDto>>(visitorAddresses);

            return Ok(visitorAddressResponseDtos);
        }

        [HttpGet("{id}", Name = "GetVisitorAddress")]
        public async Task<IActionResult> GetVisitorAddress(Guid id)
        {
            var visitorAddress = await _visitorAddressService.GetAsync(u => u.Id == id);

            if (visitorAddress == null)
            {
                return NotFound();
            }

            var visitorAddressResponseDto = _mapper.Map<VisitorAddressResponseDto>(visitorAddress);

            return Ok(visitorAddressResponseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVisitorAddress(Guid id)
        {
            var visitorAddressToDelete = await _visitorAddressService.GetAsync(u => u.Id == id);

            if (visitorAddressToDelete == null)
            {
                return NotFound();
            }

            await _visitorAddressService.RemoveAsync(visitorAddressToDelete);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateVisitorAddress([FromBody] VisitorAddressRequestDto visitorAddressRequestDto)
        {
            if (visitorAddressRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            var visitorAddressToAdd = _mapper.Map<VisitorAddress>(visitorAddressRequestDto);

            await _visitorAddressService.AddAsync(visitorAddressToAdd);

            return CreatedAtRoute("GetCity", visitorAddressToAdd, new { id = visitorAddressToAdd.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVisitorAddress(Guid id, [FromBody] VisitorAddressRequestDto visitorAddressRequestDto)
        {
            var visitorAddressToUpdate = await _visitorAddressService.GetAsync(c => c.Id == id);

            if (visitorAddressToUpdate == null)
            {
                return NotFound();
            }

            if (visitorAddressRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            _mapper.Map(visitorAddressRequestDto, visitorAddressToUpdate);

            await _visitorAddressService.UpdateAsync(visitorAddressToUpdate);

            return NoContent();
        }
    }
}
