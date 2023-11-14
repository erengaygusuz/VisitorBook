
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

            var visitorAddressGetResponseDtos = _mapper.Map<List<VisitorAddressGetResponseDto>>(visitorAddresses);

            return Ok(visitorAddressGetResponseDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVisitorAddress(Guid id)
        {
            var visitorAddress = await _visitorAddressService.GetAsync(u => u.Id == id);

            if (visitorAddress == null)
            {
                return NotFound();
            }

            var visitorAddressGetResponseDto = _mapper.Map<VisitorAddressGetResponseDto>(visitorAddress);

            return Ok(visitorAddressGetResponseDto);
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
        public async Task<IActionResult> CreateVisitorAddress([FromBody] VisitorAddressAddRequestDto visitorAddressAddRequestDto)
        {
            if (visitorAddressAddRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            var visitorAddressToAdd = _mapper.Map<VisitorAddress>(visitorAddressAddRequestDto);

            await _visitorAddressService.AddAsync(visitorAddressToAdd);

            return CreatedAtRoute("GetCity", visitorAddressToAdd, new { id = visitorAddressToAdd.Id });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateVisitorAddress([FromBody] VisitorAddressUpdateRequestDto visitorAddressUpdateRequestDto)
        {
            if (visitorAddressUpdateRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            var visitorAddressToUpdate = _mapper.Map<VisitorAddress>(visitorAddressUpdateRequestDto);

            if (visitorAddressToUpdate == null)
            {
                return NotFound();
            }

            await _visitorAddressService.UpdateAsync(visitorAddressToUpdate);

            return NoContent();
        }
    }
}
