using Moq;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Interfaces.Repositories;
using SoccerManager.Domain.Validations;

namespace SoccerManager.Domain.Tests.Validations
{
    public class TransferListCancelValidationTests
    {
        [Fact]
        public async Task TransferListCancelValidation_ValidData_ReturnValid()
        {
            // Arrange
            var transferListRepositoryMock = new Mock<ITransferListRepository>();
            transferListRepositoryMock.Setup(r => r.HasOpenByTransferIdAsync(It.IsAny<long>(), It.IsAny<long>())).Returns(Task.FromResult(true));

            var entity = TransferListEntity.Factory.CreateToCancelTransfer(1, 1);

            // Act
            var validationResult = await new TransferListCancelValidation(transferListRepositoryMock.Object).ValidateAsync(entity);

            // Assert
            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task TransferListCancelValidation_DoesntExistTransfer_ReturnError()
        {
            // Arrange            
            var transferListRepositoryMock = new Mock<ITransferListRepository>();
            transferListRepositoryMock.Setup(r => r.HasOpenByTransferIdAsync(It.IsAny<long>(), It.IsAny<long>())).Returns(Task.FromResult(false));

            var entity = TransferListEntity.Factory.CreateToCancelTransfer(1, 1);

            // Act
            var validationResult = await new TransferListCancelValidation(transferListRepositoryMock.Object).ValidateAsync(entity);

            // Assert
            Assert.True(!validationResult.IsValid);
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "This transfer does not belong to you or does not exist."));
            Assert.Equal(1, validationResult?.Errors.Count);
        }

        [Fact]
        public async Task TransferListCancelValidation_InvalidData_ReturnErrors()
        {
            // Arrange
            var transferListRepositoryMock = new Mock<ITransferListRepository>();
            transferListRepositoryMock.Setup(r => r.HasOpenAsync(It.IsAny<long>(), It.IsAny<long>())).Returns(Task.FromResult(false));

            var entity = new TransferListEntity();

            // Act
            var validationResult = await new TransferListCancelValidation(transferListRepositoryMock.Object).ValidateAsync(entity);

            // Assert
            Assert.True(!validationResult.IsValid);
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'Id' must be greater than '0'."));            
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'Source Team Id' must be greater than '0'."));            
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'Updated At' must be greater than '01/01/1900 00:00:00'."));
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'Status Id' must be equal to '3'."));
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "This transfer does not belong to you or does not exist."));
            Assert.Equal(5, validationResult?.Errors.Count);
        }
    }
}
