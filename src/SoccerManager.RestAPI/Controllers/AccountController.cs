using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SoccerManager.Domain.Interfaces.Services;
using SoccerManager.RestAPI.ApiInputs;
using SoccerManager.RestAPI.ApiResponses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SoccerManager.RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AccountController(IConfiguration configuration, IUserService userService)
        {
            this._configuration = configuration;
            this._userService = userService;
        }

        /// <summary>
        /// Register a new user, create a team with players and budget of 5,000,000
        /// </summary>
        /// <param name="input"></param>        
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/SignUp
        ///     {
        ///         "username": "username@gmail.com",
        ///         "password": "P@ssw0rd*"
        ///     }
        /// </remarks>
        /// <response code="200">User created successfully</response>
        /// <response code="400">If there are any error messages. E.g., The username already exists</response>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("SignUp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostSignUp([FromBody] SignUpInput input)
        {
            var validationResult = await _userService.SignUpAsync(input.Username ?? "", input.Password ?? "");
            validationResult?.AddToModelState(ModelState, null);
            if (ModelState.IsValid)
            {
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Checks the username and password and returns a bearer token to allow access to secure endpoints
        /// </summary>
        /// <param name="input"></param>        
        /// <remarks>
        /// Sample request:        
        ///     POST /api/Login
        ///     {
        ///         "username": "username@gmail.com",
        ///         "password": "P@ssw0rd*"
        ///     }
        /// </remarks>
        /// <response code="200">Returns a bearer token to be used in the secure endpoints</response>
        /// <response code="400">If there are any error messages. E.g., Username or Password invalid</response>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType(typeof(LoginReponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostLogin([FromBody] LoginInput input)
        {
            var user = await _userService.GetByUsernameAndPasswordAsync(input.Username ?? "", input.Password ?? "");
            if (user == null)
            {
                ModelState.AddModelError("Username/Password", "Username or Password invalid.");
                return BadRequest(ModelState);
            }

            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = GetToken(authClaims);
            return Ok(new LoginReponse(new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo));
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(_configuration.GetValue<int>("JWT:TokenDurationInHours")),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
