using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;

    public AddressService(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<Result<IEnumerable<AddressDto>>> GetAllAddressesAsync()
    {
        var addresses = await _addressRepository.GetAllAsync();
        var addressDtos = addresses.Select(MapToDto);
        return Result.Success(addressDtos);
    }

    public async Task<Result<AddressDto>> GetAddressByIdAsync(Guid id)
    {
        var address = await _addressRepository.GetByIdAsync(id);
        
        if (address == null)
            return Result.Failure<AddressDto>($"Address with ID {id} not found.");

        return Result.Success(MapToDto(address));
    }

    public async Task<Result<AddressDto>> CreateAddressAsync(CreateAddressDto createAddressDto)
    {
        if (string.IsNullOrWhiteSpace(createAddressDto.Street))
            return Result.Failure<AddressDto>("Street is required.");
        
        if (string.IsNullOrWhiteSpace(createAddressDto.City))
            return Result.Failure<AddressDto>("City is required.");

        var address = new Address
        {
            Id = Guid.NewGuid(),
            Street = createAddressDto.Street,
            City = createAddressDto.City,
            State = createAddressDto.State,
            ZipCode = createAddressDto.ZipCode,
            Country = createAddressDto.Country,
            CreatedAt = DateTime.UtcNow
        };

        var createdAddress = await _addressRepository.CreateAsync(address);
        return Result.Success(MapToDto(createdAddress));
    }

    public async Task<Result<AddressDto>> UpdateAddressAsync(Guid id, UpdateAddressDto updateAddressDto)
    {
        var existingAddress = await _addressRepository.GetByIdAsync(id);
        if (existingAddress == null)
            return Result.Failure<AddressDto>($"Address with ID {id} not found.");

        if (string.IsNullOrWhiteSpace(updateAddressDto.Street))
            return Result.Failure<AddressDto>("Street is required.");
        
        if (string.IsNullOrWhiteSpace(updateAddressDto.City))
            return Result.Failure<AddressDto>("City is required.");

        existingAddress.Street = updateAddressDto.Street;
        existingAddress.City = updateAddressDto.City;
        existingAddress.State = updateAddressDto.State;
        existingAddress.ZipCode = updateAddressDto.ZipCode;
        existingAddress.Country = updateAddressDto.Country;
        existingAddress.UpdatedAt = DateTime.UtcNow;

        var updatedAddress = await _addressRepository.UpdateAsync(existingAddress);
        
        if (updatedAddress == null)
            return Result.Failure<AddressDto>("Failed to update address.");

        return Result.Success(MapToDto(updatedAddress));
    }

    public async Task<Result> DeleteAddressAsync(Guid id)
    {
        var exists = await _addressRepository.ExistsAsync(id);
        if (!exists)
            return Result.Failure($"Address with ID {id} not found.");

        var deleted = await _addressRepository.DeleteAsync(id);
        
        if (!deleted)
            return Result.Failure("Failed to delete address.");

        return Result.Success();
    }

    private static AddressDto MapToDto(Address address)
    {
        return new AddressDto
        {
            Id = address.Id,
            Street = address.Street,
            City = address.City,
            State = address.State,
            ZipCode = address.ZipCode,
            Country = address.Country
        };
    }
}
