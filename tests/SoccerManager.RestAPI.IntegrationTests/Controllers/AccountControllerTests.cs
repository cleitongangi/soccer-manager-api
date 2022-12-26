using SoccerManager.RestAPI.ApiInputs;
using SoccerManager.RestAPI.IntegrationTests.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SoccerManager.RestAPI.IntegrationTests.Controllers
{
    public class AccountControllerTests
    {
        public AccountControllerTests()
        {
            using var application = new RestApiApplicationFactory();
            var client = application.CreateClient();

            var input = new LoginInput
            {
                Username = "user1@gmail.com",
                Password = "Password*"
            };

            var result = client.PostAsJsonAsync<LoginInput>("/api/Account/Login", input).Result;

        }

        [Fact]
        public async Task PostSignUp_ReturnSuccess()
        {
            // Arrange
            await using var application = new RestApiApplicationFactory();
            await new DbRepository(application).ResetDbAsync();

            var input = new SignUpInput { Username = "newuser@gmail.com", Password = "Password*" };

            // Act
            var client = application.CreateClient();
            var result = await client.PostAsJsonAsync<SignUpInput>("/api/Account/SignUp", input);

            // Assert
            result.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task PostLogin_ReturnSuccess()
        {
            // Arrange
            await using var application = new RestApiApplicationFactory();
            await new DbRepository(application).ResetDbAsync();

            var client = application.CreateClient();

            var input = new LoginInput { Username = "tests@gmail.com", Password = "Password*" };
            // Act
            var result = await client.PostAsJsonAsync<LoginInput>("/api/Account/Login", input);

            // Assert
            result.EnsureSuccessStatusCode();
        }
    }
}
