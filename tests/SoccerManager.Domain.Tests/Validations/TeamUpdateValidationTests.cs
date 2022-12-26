using Moq;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Interfaces.Repositories;
using SoccerManager.Domain.Validations;

namespace SoccerManager.Domain.Tests.Validations
{
    public class TeamUpdateValidationTests
    {
        [Fact]
        public async Task TeamUpdateValidation_ValidTeam_ReturnValid()
        {
            // Arrange            
            var entity = new TeamEntity(1, "Team Name", "Brazil");

            // Act
            var validationResult = await new TeamUpdateValidation().ValidateAsync(entity);

            // Assert
            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task TeamUpdateValidation_BlankTeam_ReturnError()
        {
            // Arrange            
            var entity = new TeamEntity();

            // Act
            var validationResult = await new TeamUpdateValidation().ValidateAsync(entity);

            // Assert
            Assert.True(!validationResult.IsValid);
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'Team Id' must be greater than '0'."));
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'Team Name' must not be empty."));
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "'Team Country' must not be empty."));
            Assert.Equal(3, validationResult?.Errors.Count);
        }

        [Fact]
        public async Task TeamUpdateValidation_FieldsGreaterMaxLength_ReturnError()
        {
            // Arrange            
            var entity = new TeamEntity(1, "123456789 123456789 123456789 123456789 123456789 1", "123456789 123456789 123456789 123456789 123456789 1");

            // Act
            var validationResult = await new TeamUpdateValidation().ValidateAsync(entity);

            // Assert
            Assert.True(!validationResult.IsValid);
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "The length of 'Team Name' must be 50 characters or fewer. You entered 51 characters."));
            Assert.True(validationResult?.Errors.Any(x => x.ErrorMessage == "The length of 'Team Country' must be 50 characters or fewer. You entered 51 characters."));
            Assert.Equal(2, validationResult?.Errors.Count);
        }
    }
}
