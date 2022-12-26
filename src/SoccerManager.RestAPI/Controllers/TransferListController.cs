using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Domain.Core.Pagination;
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
    public class TransferListController : AppControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITransferListRepository _transferListRepository;
        private readonly ITransferListService _transferListService;

        public TransferListController(IMapper mapper, ITransferListRepository transferListRepository, ITransferListService transferListService)
        {
            this._mapper = mapper;
            this._transferListRepository = transferListRepository;
            this._transferListService = transferListService;
        }

        /// <summary>
        /// Add a player to a transfer list
        /// </summary>
        /// <param name="input"></param>
        /// <remarks>
        /// Sample request:        
        ///     POST /api/TransferList
        ///     {
        ///         "PlayerId": 10,
        ///         "Price": 1500000
        ///     }
        /// </remarks>
        /// <response code="201">Player added in transfer list successfully</response>
        /// <response code="401">Was not possible to get the logged user id</response>        
        /// <response code="400">If there are any error messages</response>
        /// <returns></returns>
        [HttpPost()]
        [ProducesResponseType(typeof(IEnumerable<GetTeamPlayerReponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostTransferList([FromBody] AddTransferListInput input)
        {
            var teamId = base.GetAuthenticatedUserId();
            if (!teamId.HasValue)
            {
                return Unauthorized();
            }

            var (validationResult, transferId) = await _transferListService.AddAsync(teamId.Value, input.PlayerId ?? 0, input.Price ?? 0);
            validationResult?.AddToModelState(ModelState, null);
            if (ModelState.IsValid)
            {
                return new CreatedAtRouteResult("GetTransferById", new { transferId }, null);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Remove a player from a transfer list
        /// </summary>
        /// <param name="transferId"></param>
        /// <remarks>
        /// Sample request:        
        ///     DELETE /api/TransferList/5
        /// </remarks>
        /// <response code="200">Player was removed from a transfer list successfully</response>
        /// <response code="401">Was not possible to get the logged user id</response>        
        /// <response code="400">If there are any error messages</response>
        /// <returns></returns>
        [HttpDelete("{transferId:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelTransfer(long? transferId)
        {
            var teamId = base.GetAuthenticatedUserId();
            if (!teamId.HasValue)
            {
                return Unauthorized();
            }

            var validationResult = await _transferListService.RemoveAsync(teamId.Value, transferId ?? 0);
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
        /// Buy a player from a transfer list
        /// </summary>
        /// <remarks>
        /// Sample request:        
        ///     PUT /api/TransferList/5/Buy        
        /// </remarks>
        /// <response code="200">The player has been successfully purchased</response>
        /// <response code="401">Was not possible to get the logged user id</response>        
        /// <response code="400">If there are any error messages</response>
        /// <returns></returns>        
        [HttpPut("{transferId}/Buy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutBuy([FromRoute] long? transferId)
        {
            var targetTeamId = base.GetAuthenticatedUserId();
            if (!targetTeamId.HasValue)
            {
                return Unauthorized();
            }

            var validationResult = await _transferListService.BuyAsync(transferId ?? 0, targetTeamId.Value);
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
        /// Get a transfer list by TransferId
        /// </summary>
        /// <remarks>
        /// Sample request:        
        ///     GET /api/TransferList/5        
        /// </remarks>
        /// <response code="200">Got the transfer list successfully</response>        
        /// <response code="404">If transfer list is not found</response>
        /// <returns></returns>
        [HttpGet("{transferId:long}", Name = "GetTransferById")]
        [ProducesResponseType(typeof(GetTransferListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTransfer(long? transferId)
        {
            var result = await _transferListRepository.GetAsync(transferId);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<GetTransferListResponse>(result));
        }

        /// <summary>
        /// Get the complete transfer list (all players)
        /// </summary>        
        /// <remarks>
        /// Sample request:        
        ///     GET /api/TransferList?page=2        
        /// </remarks>
        /// <response code="200">Got the transfer list successfully</response>
        /// <returns></returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(PagedResult<GetTransferListResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTransfer(int page = 1)
        {
            return await GetTransfer(null, page);
        }

        /// <summary>
        /// Get the transfer list from a teamId
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="page"></param>
        /// <remarks>
        /// Sample request:        
        ///     GET /api/Teams/1/TransferList?page=2        
        /// </remarks>
        /// <response code="200">Got the transfer list successfully</response>
        /// <returns></returns>
        [HttpGet("/api/Teams/{teamId:long}/TransferList")]
        [ProducesResponseType(typeof(PagedResult<GetTransferListResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTransfer(long? teamId, int page = 1)
        {
            var result = await _transferListRepository.GetAsync(teamId, page);
            if (result.Results.Count == 0)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PagedResult<GetTransferListResponse>>(result));
        }
    }
}
