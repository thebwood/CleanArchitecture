using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Application.Interfaces;

public interface IAddressService
{
    Task<Result<IEnumerable<AddressDto>>> GetAllAddressesAsync();
    Task<Result<AddressDto>> GetAddressByIdAsync(Guid id);
    Task<Result<AddressDto>> CreateAddressAsync(CreateAddressDto createAddressDto);
    Task<Result<AddressDto>> UpdateAddressAsync(Guid id, UpdateAddressDto updateAddressDto);
    Task<Result> DeleteAddressAsync(Guid id);
}
