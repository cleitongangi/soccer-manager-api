using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Interfaces.Repositories;
using SoccerManager.Domain.Interfaces.Services;
using SoccerManager.RestAPI.ApiInputs;
using SoccerManager.RestAPI.ApiResponses;
using SoccerManager.RestAPI.Controllers.Base;

namespace SoccerManager.RestAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : AppControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITeamService _teamService;
        private readonly IPlayerService _playerService;
        private readonly ITeamRepository _teamRepository;
        private readonly IPlayerRepository _playerRepository;

        public TeamsController(IMapper mapper, ITeamService teamService, IPlayerService playerService, ITeamRepository teamRepository, IPlayerRepository playerRepository)
        {
            this._mapper = mapper;
            this._teamService = teamService;
            this._playerService = playerService;
            this._teamRepository = teamRepository;
            this._playerRepository = playerRepository;
        }

        /// <summary>
        /// Get logged user team details
        /// </summary>
        /// <remarks>
        /// Sample request:        
        ///     GET /api/Teams        
        /// </remarks>
        /// <response code="200">Returns the logged user team information</response>
        /// <response code="401">This returns happen if is not possible get the logged user id</response>
        /// <returns></returns>
        [HttpGet()]
        [ProducesResponseType(typeof(GetTeamResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]        
        public async Task<IActionResult> GetTeam()
        {
            return await GetTeam(null);
        }

        /// <summary>
        /// Get team details by teamId
        /// </summary>        
        /// <param name="teamId">TeamId to find the team</param>
        /// <remarks>
        /// Sample request:        
        ///     GET /api/Teams        
        /// </remarks>
        /// <response code="200">Returns the logged user team information</response>
        /// <response code="401">This returns happen if is not possible get the logged user id</response>
        /// <response code="404">TeamId not found</response>
        /// <returns></returns>
        [HttpGet("{teamId}")]
        [ProducesResponseType(typeof(GetTeamResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTeam(long? teamId)
        {
            if (!teamId.HasValue)
            { // Get user Authenticated
                var userId = base.GetAuthenticatedUserId();
                if (userId == null)
                {
                    return Unauthorized();
                }
                teamId = userId.Value; // UserId and teamId is the same, because is one team to one user
            }

            var teamDetails = await _teamRepository.GetTeamDetailsAsync(teamId.Value);
            if (teamDetails == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<GetTeamResponse>(teamDetails));
        }

        /// <summary>
        /// Update team information
        /// </summary>
        /// <param name="input">Team information to update</param>
        /// <remarks>
        /// Sample request:        
        ///     PUT /api/Teams
        ///     {
        ///         "teamName": "Team Name Test",
        ///         "teamCountry": "Brazil"
        ///     }
        /// </remarks>
        /// <response code="200">Team information updated successfully</response>
        /// <response code="401">Was not possible to get the logged user id</response>
        /// <response code="404">TeamId not found</response>
        /// <response code="400">If there are any error messages</response>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(IEnumerable<GetTeamPlayerReponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutTeam([FromBody] UpdateTeamInput input)
        {
            var teamId = base.GetAuthenticatedUserId();
            if (!teamId.HasValue)
            {
                return Unauthorized();
            }

            var validationResult = await _teamService.UpdateAsync(teamId.Value, input.TeamName ?? "", input.TeamCountry ?? "");

            if (validationResult.Errors.Any(x => x.PropertyName == "notfound"))
            {
                return NotFound();
            }

            if (validationResult.IsValid)
            {
                return Ok();
            }
            else
            {
                validationResult?.AddToModelState(ModelState, null);
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Get team players
        /// </summary>
        /// <param name="teamId">TeamId to find the team</param>
        /// <remarks>
        /// Sample request:        
        ///     GET /api/Teams/1/Players        
        /// </remarks>
        /// <response code="200">Returns the team players</response>
        /// <response code="404">TeamId not found</response>
        /// <returns></returns>
        [HttpGet("{teamId}/Players")]
        [ProducesResponseType(typeof(IEnumerable<GetTeamPlayerReponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPlayers(long teamId)
        {
            var players = await _playerRepository.GetTeamPlayersAsync(teamId);
            if (players.Count() == 0)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<GetTeamPlayerReponse>>(players));
        }

        /// <summary>
        /// Update a player
        /// </summary>
        /// <param name="playerId">PlayerId to be updated</param>
        /// <param name="input">Player information to update</param>
        /// <remarks>
        /// Sample request:        
        ///     PUT /api/Teams/Players/1
        ///     {
        ///         "firstName": "Neymar",
        ///         "lastName": "Junior",
        ///         "country": "Brazil"
        ///     }
        /// </remarks>
        /// <response code="200">Player information updated successfully</response>
        /// <response code="401">Was not possible to get the logged user id</response>
        /// <response code="404">PlayerId not found</response>
        /// <response code="400">If there are any error messages</response>
        /// <returns></returns>
        [HttpPut("Players/{playerId}")]
        [ProducesResponseType(typeof(IEnumerable<GetTeamPlayerReponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutPlayers([FromRoute] long playerId, [FromBody] UpdatePlayerInput input)
        {
            var teamId = base.GetAuthenticatedUserId();
            if (!teamId.HasValue)
            {
                return Unauthorized();
            }

            var playerEntity = PlayerEntity.Factory.NewForUpdate(playerId, input.FirstName, input.LastName, input.Country);
            var validationResult = await _playerService.UpdatePlayerAsync(teamId.Value, playerEntity);

            if (validationResult.Errors.Any(x => x.PropertyName == "notfound"))
            {
                return NotFound();
            }
                        
            if (validationResult.IsValid)
            {
                return Ok();
            }
            else
            {
                validationResult?.AddToModelState(ModelState, null);
                return BadRequest(ModelState);
            }
        }
    }
}
