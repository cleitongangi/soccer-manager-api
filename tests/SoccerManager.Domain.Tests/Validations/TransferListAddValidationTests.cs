using Moq;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Interfaces.Repositories;
using SoccerManager.Domain.Validations;

namespace SoccerManager.Domain.Tests.Validations
{
    public class TransferListAddValidationTests
    {
        [Fact]
        public async Task TransferListAddValidation_ValidData_ReturnValid()
        {
            // Arrange
            var playerRepositoryMock = new Mock<IPlayerRepository>();
            playerRepositoryMock.Setup(r => r.HasTeamPlayerActiveAsync(It.IsAny<long>(), It.IsAny<long>())).Returns(Task.FromResult(true));

            var transferListRepositoryMock = new Mock<ITransferListRepository>();
            transferListRepositoryMock.Setup(r => r.HasOpenAsync(It.IsAny<long>(), It.IsAny<long>())).Returns(Task.FromResult(false));

            var entity = TransferListEntity.Factory.NewTransfer(1, 1, 2000000);

            // Act
            var validationResult = await new TransferListAddValidation(playerRepositoryMock.Object, transferListRepositoryMock.Object).ValidateAsync(entity);

            // Assert
            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task TransferListAddValidation_ExistingTransfer_ReturnError()
        {
            // Arrange
            var playerRepositoryMock = new Mock<IPlayerRepository>();
            playerRepositoryMock.Setup(r => r.HasTeamPlayerActiveAsync(It.IsAny<long>(), It.IsAny<long>())).Returns(Task.FromResult(true));

            var transferListRepositoryMock = new Mock<ITransferListRepository>();
            transferListRepositoryMock.Setup(r => r.HasOpenAsync(It.IsAny<long>(), It.IsAny<long>())).Returns(Task.FromResult(true));

            var entity = TransferListEntity.Factory.NewTransfer(1, 1, 2000000);

            // Act
            var validationResult = await new TransferListAddValidation(playerRepositoryMock.Object, transferListRepositoryMock.Object).ValidateAsync(entity);

            // Assert
            Assert.True(!validationResult.IsValid);
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "The player is already on the transfer list."));
            Assert.Equal(1, validationResult?.Errors.Count);
        }

        [Fact]
        public async Task TransferListAddValidation_PlayerDoesntBelongsToTeam_ReturnError()
        {
            // Arrange
            var playerRepositoryMock = new Mock<IPlayerRepository>();
            playerRepositoryMock.Setup(r => r.HasTeamPlayerActiveAsync(It.IsAny<long>(), It.IsAny<long>())).Returns(Task.FromResult(false));

            var transferListRepositoryMock = new Mock<ITransferListRepository>();
            transferListRepositoryMock.Setup(r => r.HasOpenAsync(It.IsAny<long>(), It.IsAny<long>())).Returns(Task.FromResult(false));

            var entity = TransferListEntity.Factory.NewTransfer(1, 1, 2000000);

            // Act
            var validationResult = await new TransferListAddValidation(playerRepositoryMock.Object, transferListRepositoryMock.Object).ValidateAsync(entity);

            // Assert
            Assert.True(!validationResult.IsValid);
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "The player does not belong to your team."));
            Assert.Equal(1, validationResult?.Errors.Count);
        }

        [Fact]
        public async Task TransferListAddValidation_InvalidData_ReturnErrors()
        {
            // Arrange
            var playerRepositoryMock = new Mock<IPlayerRepository>();
            playerRepositoryMock.Setup(r => r.HasTeamPlayerActiveAsync(It.IsAny<long>(), It.IsAny<long>())).Returns(Task.FromResult(true));

            var transferListRepositoryMock = new Mock<ITransferListRepository>();
            transferListRepositoryMock.Setup(r => r.HasOpenAsync(It.IsAny<long>(), It.IsAny<long>())).Returns(Task.FromResult(false));

            var entity = new TransferListEntity();

            // Act
            var validationResult = await new TransferListAddValidation(playerRepositoryMock.Object, transferListRepositoryMock.Object).ValidateAsync(entity);

            // Assert
            Assert.True(!validationResult.IsValid);
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'Player Id' must be greater than '0'."));
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'Created At' must be greater than '01/01/1900 00:00:00'."));
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'Source Team Id' must be greater than '0'."));
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'Price' must be greater than '0'."));
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'Updated At' must be greater than '01/01/1900 00:00:00'."));
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'Status Id' must be equal to '1'."));
            Assert.Equal(6, validationResult?.Errors.Count);
        }
    }
}
