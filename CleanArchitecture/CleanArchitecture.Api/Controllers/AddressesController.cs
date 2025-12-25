using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers
{
    /// <summary>
    /// Manages address operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly IValidator<CreateAddressDto> _createValidator;
        private readonly IValidator<UpdateAddressDto> _updateValidator;

        public AddressesController(
            IAddressService addressService,
            IValidator<CreateAddressDto> createValidator,
            IValidator<UpdateAddressDto> updateValidator)
        {
            _addressService = addressService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        /// <summary>
        /// Retrieves all addresses
        /// </summary>
        /// <returns>A list of all addresses</returns>
        /// <response code="200">Returns the list of addresses</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AddressDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AddressDto>>> GetAll()
        {
            var result = await _addressService.GetAllAddressesAsync();
            
            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        /// <summary>
        /// Retrieves a specific address by ID
        /// </summary>
        /// <param name="id">The address ID (GUID)</param>
        /// <returns>The requested address</returns>
        /// <response code="200">Returns the requested address</response>
        /// <response code="404">If the address is not found</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(AddressDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AddressDto>> GetById(Guid id)
        {
            var result = await _addressService.GetAddressByIdAsync(id);
            
            if (result.IsFailure)
                return NotFound(result.Error);

            return Ok(result.Value);
        }

        /// <summary>
        /// Creates a new address
        /// </summary>
        /// <param name="createAddressDto">The address data</param>
        /// <returns>The newly created address</returns>
        /// <response code="201">Returns the newly created address</response>
        /// <response code="400">If the request is invalid</response>
        [HttpPost]
        [ProducesResponseType(typeof(AddressDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AddressDto>> Create([FromBody] CreateAddressDto createAddressDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createAddressDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            var result = await _addressService.CreateAddressAsync(createAddressDto);
            
            if (result.IsFailure)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
        }

        /// <summary>
        /// Updates an existing address
        /// </summary>
        /// <param name="id">The address ID (GUID)</param>
        /// <param name="updateAddressDto">The updated address data</param>
        /// <returns>The updated address</returns>
        /// <response code="200">Returns the updated address</response>
        /// <response code="400">If the request is invalid</response>
        /// <response code="404">If the address is not found</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(AddressDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AddressDto>> Update(Guid id, [FromBody] UpdateAddressDto updateAddressDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateAddressDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            var result = await _addressService.UpdateAddressAsync(id, updateAddressDto);
            
            if (result.IsFailure)
                return NotFound(result.Error);

            return Ok(result.Value);
        }

        /// <summary>
        /// Deletes a specific address
        /// </summary>
        /// <param name="id">The address ID (GUID)</param>
        /// <returns>No content</returns>
        /// <response code="204">If the address was successfully deleted</response>
        /// <response code="404">If the address is not found</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _addressService.DeleteAddressAsync(id);
            
            if (result.IsFailure)
                return NotFound(result.Error);

            return NoContent();
        }
    }
}
