using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Validations;

namespace SoccerManager.Domain.Tests.Validations
{
    public class PlayerUpdateValidationTests
    {
        [Fact]
        public async Task PlayerUpdateValidation_ValidTeam_ReturnValid()
        {
            // Arrange            
            var entity = PlayerEntity.Factory.NewForUpdate(1, "Cleiton", "Gangi", "Brazil");

            // Act
            var validationResult = await new PlayerUpdateValidation().ValidateAsync(entity);

            // Assert
            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task PlayerUpdateValidation_BlankPlayer_ReturnError()
        {
            // Arrange            
            var entity = new PlayerEntity();

            // Act
            var validationResult = await new PlayerUpdateValidation().ValidateAsync(entity);

            // Assert
            Assert.True(!validationResult.IsValid);
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'Id' must be greater than '0'."));
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'First Name' must not be empty."));
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'Last Name' must not be empty."));
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'Country' must not be empty."));
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'Updated At' must be greater than '01/01/1900 00:00:00'."));
            Assert.Equal(5, validationResult?.Errors.Count);
        }

        [Fact]
        public async Task PlayerUpdateValidation_FieldsGreaterMaxLength_ReturnError()
        {
            // Arrange            
            var entity = PlayerEntity.Factory.NewForUpdate(1, new string('x', 51), new string('x', 51), new string('x', 51));

            // Act
            var validationResult = await new PlayerUpdateValidation().ValidateAsync(entity);

            // Assert
            Assert.True(!validationResult.IsValid);
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "The length of 'First Name' must be 50 characters or fewer. You entered 51 characters."));
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "The length of 'Last Name' must be 50 characters or fewer. You entered 51 characters."));
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "The length of 'Country' must be 50 characters or fewer. You entered 51 characters."));
            Assert.Equal(3, validationResult?.Errors.Count);
        }
    }
}
