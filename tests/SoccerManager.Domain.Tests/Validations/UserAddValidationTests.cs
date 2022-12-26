using Moq;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Interfaces.Repositories;
using SoccerManager.Domain.Validations;

namespace SoccerManager.Domain.Tests.Validations
{
    public class UserAddValidationTests
    {
        [Fact]
        public async Task UserAddValidation_ValidUser_ReturnValid()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.HasAsync(It.IsAny<string>())).Returns(Task.FromResult(false));

            var entity = new UserEntity("teste@gmail.com", "Password*", DateTime.Now);

            // Act
            var validationResult = await new UserAddValidation(userRepositoryMock.Object).ValidateAsync(entity);

            // Assert
            Assert.True(validationResult.IsValid);            
        }

        [Fact]
        public async Task UserAddValidation_ExistingUser_ReturnError()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.HasAsync(It.IsAny<string>())).Returns(Task.FromResult(true));

            var entity = new UserEntity("teste@gmail.com", "Password*", DateTime.Now);

            // Act
            var validationResult = await new UserAddValidation(userRepositoryMock.Object).ValidateAsync(entity);

            // Assert
            Assert.True(!validationResult.IsValid);
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "The username already exists."));
            Assert.Equal(1, validationResult?.Errors.Count);
        }

        [Fact]
        public async Task UserAddValidation_InvalidUser_ReturnErrors()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.HasAsync(It.IsAny<string>())).Returns(Task.FromResult(false));

            var entity = new UserEntity("", "", DateTime.MinValue);

            // Act
            var validationResult = await new UserAddValidation(userRepositoryMock.Object).ValidateAsync(entity);

            // Assert
            Assert.True(!validationResult.IsValid);
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'Created At' must be greater than '01/01/1900 00:00:00'."));
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'Username' must not be empty."));
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'Username' is not a valid email address."));
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'Password' must not be empty."));
            Assert.Equal(4, validationResult?.Errors.Count);
        }
    }
}
