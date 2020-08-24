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
    public class TimeWindowController : BaseController
    {
        private readonly ITimeWindowService _timeWindowService;
        private readonly IFeatureService _featureService;

        public TimeWindowController(ITimeWindowService timeWindowService, IFeatureService featureService)
        {
            _timeWindowService = timeWindowService ?? throw new ArgumentNullException(nameof(timeWindowService));
            _featureService = featureService ?? throw new ArgumentNullException(nameof(featureService));
        }

        /// <summary>
        /// Get all Time Windows
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetAllTimeWindows")]
        [ProducesResponseType(typeof(IList<TimeWindow>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            IList<TimeWindow> timeWindows = await _timeWindowService
                .GetAllAsync()
                .ConfigureAwait(false);

            return Ok(timeWindows);
        }

        /// <summary>
        /// Get Time Window by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetTimeWindowById")]
        [ProducesResponseType(typeof(TimeWindow), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(CreateProblemDetailsResponse("Invalid Id"));
            }

            TimeWindow timeWindow = await _timeWindowService
                .GetByIdAsync(id)
                .ConfigureAwait(false);

            return timeWindow == null
                ? (IActionResult) NotFound()
                : Ok(timeWindow);
        }

        /// <summary>
        /// Get Time Window by feature Id
        /// </summary>
        /// <param name="featureId"></param>
        /// <returns></returns>
        [HttpGet("[action]/{featureId}", Name = "GetTimeWindowByFeatureId")]
        [ProducesResponseType(typeof(TimeWindow), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByFeatureId(Guid featureId)
        {
            if (featureId == Guid.Empty)
            {
                return BadRequest(CreateProblemDetailsResponse("Invalid Id"));
            }

            TimeWindow timeWindow = await _timeWindowService
                .GetByFeatureId(featureId)
                .ConfigureAwait(false);

            return timeWindow == null
                ? (IActionResult) NotFound()
                : Ok(timeWindow);
        }

        /// <summary>
        /// Create a new Time Window
        /// </summary>
        /// <param name="timeWindow"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost(Name = "PostTimeWindow")]
        [ProducesResponseType(typeof(TimeWindow), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(TimeWindow timeWindow, CancellationToken cancellationToken = default)
        {
            if (timeWindow == null)
            {
                return BadRequest(CreateProblemDetailsResponse("TimeWindow is required"));
            }

            Feature timeWindowFeature = await _featureService
                .GetByIdAsync(timeWindow.FeatureId)
                .ConfigureAwait(false);

            if (timeWindowFeature == null)
            {
                return BadRequest(CreateProblemDetailsResponse("Invalid feature id"));
            }

            TimeWindow existingFeatureTimeWindow =
                await _timeWindowService.GetByFeatureId(timeWindow.FeatureId, cancellationToken)
                    .ConfigureAwait(false);

            if (existingFeatureTimeWindow != null)
            {
                return BadRequest(CreateProblemDetailsResponse("Feature already has a time window"));
            }

            timeWindow.CreatedOn = DateTimeOffset.Now;

            TimeWindow newTimeWindow = await _timeWindowService
                .AddAsync(timeWindow)
                .ConfigureAwait(false);

            return newTimeWindow == null
                ? (IActionResult) StatusCode(StatusCodes.Status500InternalServerError)
                : Ok(newTimeWindow);
        }

        /// <summary>
        /// Update a Time Window
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timeWindow"></param>
        /// <returns></returns>
        [HttpPut("{id}", Name = "PutTimeWindow")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(TimeWindow), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put(Guid id, TimeWindow timeWindow)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(CreateProblemDetailsResponse("Invalid Id"));
            }

            if (timeWindow == null)
            {
                return BadRequest(CreateProblemDetailsResponse("TimeWindow is required"));
            }

            TimeWindow existingTimeWindow = await _timeWindowService
                .GetByIdAsync(id)
                .ConfigureAwait(false);

            if (existingTimeWindow == null)
            {
                return NotFound();
            }

            if (existingTimeWindow.Id != timeWindow.Id)
            {
                return BadRequest(CreateProblemDetailsResponse("TimeWindow Id mismatch"));
            }

            timeWindow.ModifiedOn = DateTimeOffset.Now;
            TimeWindow updatedTimeWindow = _timeWindowService.Update(timeWindow);
            return updatedTimeWindow == null
                ? (IActionResult) StatusCode(StatusCodes.Status500InternalServerError)
                : Ok(updatedTimeWindow);
        }

        /// <summary>
        /// Delete a Time Window
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete(Name = "DeleteTimeWindow")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(CreateProblemDetailsResponse("Invalid Id"));
            }

            _timeWindowService.DeleteById(id);
            TimeWindow timeWindow = await _timeWindowService.GetByIdAsync(id);
            return timeWindow == null
                ? StatusCode(StatusCodes.Status500InternalServerError)
                : Ok();
        }
    }
}