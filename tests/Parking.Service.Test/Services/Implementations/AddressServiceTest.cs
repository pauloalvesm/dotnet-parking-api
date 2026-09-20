using MapsterMapper;
using Moq;
using Parking.Domain.Entities;
using Parking.Domain.Interfaces.Repositories;
using Parking.Service.DTOs;
using Parking.Service.Implementations;
using Parking.Test.Shared.Helpers;

namespace Parking.Service.Test.Services.Implementations;

public class AddressServiceTest
{
    private readonly Mock<IAddressRepository> _addressRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AddressService _addressService;

    public AddressServiceTest()
    {
        _addressRepositoryMock = new Mock<IAddressRepository>();
        _mapperMock = new Mock<IMapper>();
        _addressService = new AddressService(_addressRepositoryMock.Object, _mapperMock.Object);
    }
    
    [Fact]
    public async Task AddressService_GetAllAddresses_ShouldReturnAllAddresses()
    {
        // Arrange
        var addressList = new List<Address> { AddressTestHelper.CreateValidAddressEntity(1) };
        var addressDtoList = new List<AddressDTO> { AddressTestHelper.CreateValidAddressDTO(1) };

        _addressRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(addressList);

        _mapperMock
            .Setup(mapper => mapper.Map<IEnumerable<AddressDTO>>(addressList))
            .Returns(addressDtoList);

        // Act
        var result = await _addressService.GetAllAddresses();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        _addressRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<IEnumerable<AddressDTO>>(addressList), Times.Once);
    }

    [Fact]
    public async Task AddressService_GetAllAddresses_ShouldThrowExceptionWhenRepositoryFails()
    {
        // Arrange
        _addressRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _addressService.GetAllAddresses());
        _addressRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task AddressService_GetAddressById_ShouldReturnAddressById()
    {
        // Arrange
        const int addressId = 1;
        var addressEntity = AddressTestHelper.CreateValidAddressEntity(addressId);
        var addressDto = AddressTestHelper.CreateValidAddressDTO(addressId);

        _addressRepositoryMock
            .Setup(repo => repo.GetByIdAsync(addressId))
            .ReturnsAsync(addressEntity);

        _mapperMock
            .Setup(mapper => mapper.Map<AddressDTO>(addressEntity))
            .Returns(addressDto);

        // Act
        var result = await _addressService.GetAddressById(addressId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(addressId, result.Id);
        _addressRepositoryMock.Verify(repo => repo.GetByIdAsync(addressId), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<AddressDTO>(addressEntity), Times.Once);
    }

    [Fact]
    public async Task AddressService_GetAddressById_ShouldReturnNullWhenNotFound()
    {
        // Arrange
        const int addressId = 999;

        _addressRepositoryMock
            .Setup(repo => repo.GetByIdAsync(addressId))
            .ReturnsAsync((Address)null!);

        _mapperMock
            .Setup(mapper => mapper.Map<AddressDTO>(It.IsAny<Address>()))
            .Returns((AddressDTO)null!);

        // Act
        var result = await _addressService.GetAddressById(addressId);

        // Assert
        Assert.Null(result);
        _addressRepositoryMock.Verify(repo => repo.GetByIdAsync(addressId), Times.Once);
    }

    [Fact]
    public async Task AddressService_GetAddressById_ShouldThrowExceptionWhenRepositoryFails()
    {
        // Arrange
        const int addressId = 1;

        _addressRepositoryMock
            .Setup(repo => repo.GetByIdAsync(addressId))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _addressService.GetAddressById(addressId));
        _addressRepositoryMock.Verify(repo => repo.GetByIdAsync(addressId), Times.Once);
    }

    [Fact]
    public async Task AddressService_CreateAddress_ShouldCreateAddress()
    {
        // Arrange
        var inputDto = AddressTestHelper.CreateValidAddressDTO(0);
        var mappedEntity = AddressTestHelper.CreateValidAddressEntity(0);
        var createdEntity = AddressTestHelper.CreateValidAddressEntity(1);
        var resultDto = AddressTestHelper.CreateValidAddressDTO(1);

        _mapperMock
            .Setup(mapper => mapper.Map<Address>(inputDto))
            .Returns(mappedEntity);

        _addressRepositoryMock
            .Setup(repo => repo.AddAsync(mappedEntity))
            .ReturnsAsync(createdEntity);

        _mapperMock
            .Setup(mapper => mapper.Map<AddressDTO>(createdEntity))
            .Returns(resultDto);

        // Act
        var result = await _addressService.CreateAddress(inputDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        _mapperMock.Verify(mapper => mapper.Map<Address>(inputDto), Times.Once);
        _addressRepositoryMock.Verify(repo => repo.AddAsync(mappedEntity), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<AddressDTO>(createdEntity), Times.Once);
    }

    [Fact]
    public async Task AddressService_CreateAddress_ShouldThrowExceptionWhenRepositoryFails()
    {
        // Arrange
        var inputDto = AddressTestHelper.CreateValidAddressDTO(1);
        var mappedEntity = AddressTestHelper.CreateValidAddressEntity(1);

        _mapperMock
            .Setup(mapper => mapper.Map<Address>(inputDto))
            .Returns(mappedEntity);

        _addressRepositoryMock
            .Setup(repo => repo.AddAsync(mappedEntity))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _addressService.CreateAddress(inputDto));
        _addressRepositoryMock.Verify(repo => repo.AddAsync(mappedEntity), Times.Once);
    }

    [Fact]
    public async Task AddressService_UpdateAddress_ShouldUpdateAddress()
    {
        // Arrange
        var inputDto = AddressTestHelper.CreateValidAddressDTO(1);
        var mappedEntity = AddressTestHelper.CreateValidAddressEntity(1);
        var updatedEntity = AddressTestHelper.CreateValidAddressEntity(1);
        var resultDto = AddressTestHelper.CreateValidAddressDTO(1);

        _mapperMock
            .Setup(mapper => mapper.Map<Address>(inputDto))
            .Returns(mappedEntity);

        _addressRepositoryMock
            .Setup(repo => repo.UpdateAsync(mappedEntity))
            .ReturnsAsync(updatedEntity);

        _mapperMock
            .Setup(mapper => mapper.Map<AddressDTO>(updatedEntity))
            .Returns(resultDto);

        // Act
        var result = await _addressService.UpdateAddress(inputDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(inputDto.Id, result.Id);
        _mapperMock.Verify(mapper => mapper.Map<Address>(inputDto), Times.Once);
        _addressRepositoryMock.Verify(repo => repo.UpdateAsync(mappedEntity), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<AddressDTO>(updatedEntity), Times.Once);
    }

    [Fact]
    public async Task AddressService_UpdateAddress_ShouldThrowExceptionWhenRepositoryFails()
    {
        // Arrange
        var inputDto = AddressTestHelper.CreateValidAddressDTO(1);
        var mappedEntity = AddressTestHelper.CreateValidAddressEntity(1);

        _mapperMock
            .Setup(mapper => mapper.Map<Address>(inputDto))
            .Returns(mappedEntity);

        _addressRepositoryMock
            .Setup(repo => repo.UpdateAsync(mappedEntity))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _addressService.UpdateAddress(inputDto));
        _addressRepositoryMock.Verify(repo => repo.UpdateAsync(mappedEntity), Times.Once);
    }

    [Fact]
    public async Task AddressService_DeleteAddress_ShouldDeleteAddress()
    {
        // Arrange
        const int addressId = 1;

        _addressRepositoryMock
            .Setup(repo => repo.DeleteAsync(addressId))
            .Returns(Task.CompletedTask);

        // Act
        await _addressService.DeleteAddress(addressId);

        // Assert
        _addressRepositoryMock.Verify(repo => repo.DeleteAsync(addressId), Times.Once);
    }

    [Fact]
    public async Task AddressService_DeleteAddress_ShouldThrowExceptionWhenRepositoryFails()
    {
        // Arrange
        const int addressId = 1;

        _addressRepositoryMock
            .Setup(repo => repo.DeleteAsync(addressId))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _addressService.DeleteAddress(addressId));
        _addressRepositoryMock.Verify(repo => repo.DeleteAsync(addressId), Times.Once);
    }
}