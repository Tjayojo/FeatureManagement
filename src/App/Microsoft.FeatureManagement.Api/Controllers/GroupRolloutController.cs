using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Service.Interfaces;

namespace Microsoft.FeatureManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class GroupRolloutController : BaseController
    {
        private readonly IGroupRolloutService _groupRolloutService;

        public GroupRolloutController(IGroupRolloutService groupRolloutService)
        {
            _groupRolloutService = groupRolloutService ?? throw new ArgumentNullException(nameof(groupRolloutService));
        }

        /// <summary>
        /// Get all Group Rollouts
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetAllGroupRollouts")]
        [ProducesResponseType(typeof(IList<GroupRollout>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            IList<GroupRollout> groupRollouts = await _groupRolloutService
                .GetAllAsync()
                .ConfigureAwait(false);

            return Ok(groupRollouts);
        }

        /// <summary>
        /// Get a Group Rollout by name
        /// </summary>
        /// <param name="groupRolloutName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("[action]/{groupRolloutName}", Name = "GetGroupRolloutByName")]
        [ProducesResponseType(typeof(GroupRollout), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string groupRolloutName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(groupRolloutName))
            {
                return BadRequest("Invalid GroupRolloutName provided");
            }

            GroupRollout groupRollout = await _groupRolloutService
                .GetByName(groupRolloutName, cancellationToken)
                .ConfigureAwait(false);
            return groupRollout == null ? (IActionResult) NotFound() : Ok(groupRollout);
        }

        /// <summary>
        /// Get Group Rollout by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetGroupRolloutById")]
        [ProducesResponseType(typeof(GroupRollout), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(CreateProblemDetailsResponse("Invalid Id"));
            }

            GroupRollout groupRollout = await _groupRolloutService
                .GetByIdAsync(id)
                .ConfigureAwait(false);

            return groupRollout == null
                ? (IActionResult) NotFound()
                : Ok(groupRollout);
        }

        /// <summary>
        /// Create a new Group Rollout
        /// </summary>
        /// <param name="groupRollout"></param>
        /// <returns></returns>
        [HttpPost(Name = "PostGroupRollout")]
        [ProducesResponseType(typeof(GroupRollout), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(GroupRollout groupRollout)
        {
            if (groupRollout == null)
            {
                return BadRequest(CreateProblemDetailsResponse("GroupRollout is required"));
            }

            GroupRollout existingGroupRolloutWithName = await _groupRolloutService
                .GetByName(groupRollout.Name)
                .ConfigureAwait(false);

            if (existingGroupRolloutWithName != null)
            {
                return BadRequest(CreateProblemDetailsResponse("GroupRollout with the same name already exists"));
            }

            groupRollout.CreatedOn = DateTimeOffset.Now;
            GroupRollout newGroupRollout = await _groupRolloutService
                .AddAsync(groupRollout)
                .ConfigureAwait(false);

            return newGroupRollout == null
                ? (IActionResult) StatusCode(StatusCodes.Status500InternalServerError)
                : Ok(newGroupRollout);
        }

        /// <summary>
        /// Update a Group Rollout
        /// </summary>
        /// <param name="id"></param>
        /// <param name="groupRollout"></param>
        /// <returns></returns>
        [HttpPut("{id}", Name = "PutGroupRollout")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(GroupRollout), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put(Guid id, GroupRollout groupRollout)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(CreateProblemDetailsResponse("Invalid Id"));
            }

            if (groupRollout == null)
            {
                return BadRequest(CreateProblemDetailsResponse("GroupRollout is required"));
            }

            GroupRollout existingGroupRollout = await _groupRolloutService
                .GetByIdAsync(id)
                .ConfigureAwait(false);

            if (existingGroupRollout == null)
            {
                return NotFound();
            }

            if (existingGroupRollout.Id != groupRollout.Id)
            {
                return BadRequest(CreateProblemDetailsResponse("GroupRollout Id mismatch"));
            }

            groupRollout.ModifiedOn = DateTimeOffset.Now;
            GroupRollout updatedGroupRollout = _groupRolloutService.Update(groupRollout);
            return updatedGroupRollout == null
                ? (IActionResult) StatusCode(StatusCodes.Status500InternalServerError)
                : Ok(updatedGroupRollout);
        }

        /// <summary>
        /// Delete a Group Rollout
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete(Name = "DeleteGroupRollout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(CreateProblemDetailsResponse("Invalid Id"));
            }

            _groupRolloutService.DeleteById(id);
            GroupRollout groupRollout = await _groupRolloutService.GetByIdAsync(id);
            return groupRollout == null
                ? StatusCode(StatusCodes.Status500InternalServerError)
                : Ok();
        }
    }
}