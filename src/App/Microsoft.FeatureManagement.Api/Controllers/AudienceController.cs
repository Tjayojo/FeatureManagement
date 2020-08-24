using System;
using System.Collections.Generic;
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
    public class AudienceController : BaseController
    {
        private readonly IAudienceService _audienceService;
        private readonly IFeatureService _featureService;

        public AudienceController(IAudienceService audienceService, IFeatureService featureService)
        {
            _audienceService = audienceService ?? throw new ArgumentNullException(nameof(audienceService));
            _featureService = featureService ?? throw new ArgumentNullException(nameof(featureService));
        }

        /// <summary>
        /// Get all audiences
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetAllAudiences")]
        [ProducesResponseType(typeof(IList<Audience>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            IList<Audience> audiences = await _audienceService
                .GetAllAsync()
                .ConfigureAwait(false);

            return Ok(audiences);
        }

        /// <summary>
        /// Get audience by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetAudienceById")]
        [ProducesResponseType(typeof(Audience), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(CreateProblemDetailsResponse("Invalid Id"));
            }

            Audience audience = await _audienceService
                .GetByIdAsync(id)
                .ConfigureAwait(false);

            return audience == null
                ? (IActionResult) NotFound()
                : Ok(audience);
        }

        /// <summary>
        /// Get audience by Feature Id
        /// </summary>
        /// <param name="featureId"></param>
        /// <returns></returns>
        [HttpGet("[action]/{featureId}", Name = "GetAudienceByFeatureId")]
        [ProducesResponseType(typeof(Audience), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByFeatureId(Guid featureId)
        {
            if (featureId == Guid.Empty)
            {
                return BadRequest(CreateProblemDetailsResponse("Invalid Id"));
            }

            Audience audience = await _audienceService
                .GetByFeatureId(featureId)
                .ConfigureAwait(false);

            return audience == null
                ? (IActionResult) NotFound()
                : Ok(audience);
        }

        /// <summary>
        /// Create a new audience
        /// </summary>
        /// <param name="audience"></param>
        /// <returns></returns>
        [HttpPost(Name = "PostAudience")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Audience), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(Audience audience)
        {
            if (audience == null)
            {
                return BadRequest(CreateProblemDetailsResponse("Audience is required"));
            }

            Feature feature = await _featureService.GetByIdAsync(audience.FeatureId)
                .ConfigureAwait(false);
            if (feature == null)
            {
                return BadRequest(CreateProblemDetailsResponse("Feature not found"));
            }

            Audience existingFeatureAudience =
                await _audienceService.GetByFeatureId(audience.FeatureId)
                    .ConfigureAwait(false);

            if (existingFeatureAudience != null)
            {
                return BadRequest(CreateProblemDetailsResponse("Feature already has an audience"));
            }

            audience.CreatedOn = DateTimeOffset.Now;
            
            Audience newAudience = await _audienceService
                .AddAsync(audience)
                .ConfigureAwait(false);

            return newAudience == null
                ? (IActionResult) StatusCode(StatusCodes.Status500InternalServerError)
                : Ok(newAudience);
        }

        /// <summary>
        /// Update a audience
        /// </summary>
        /// <param name="id"></param>
        /// <param name="audience"></param>
        /// <returns></returns>
        [HttpPut("{id}", Name = "PutAudience")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Audience), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put(Guid id, Audience audience)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(CreateProblemDetailsResponse("Invalid Id"));
            }

            if (audience == null)
            {
                return BadRequest(CreateProblemDetailsResponse("Audience is required"));
            }

            Audience existingAudience = await _audienceService
                .GetByIdAsync(id)
                .ConfigureAwait(false);

            if (existingAudience == null)
            {
                return NotFound();
            }

            if (existingAudience.Id != audience.Id)
            {
                return BadRequest(CreateProblemDetailsResponse("Audience Id mismatch"));
            }
            
            audience.ModifiedOn = DateTimeOffset.Now;

            Audience updatedAudience = _audienceService.Update(audience);
            return updatedAudience == null
                ? (IActionResult) StatusCode(StatusCodes.Status500InternalServerError)
                : Ok(updatedAudience);
        }

        /// <summary>
        /// Delete a audience
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete(Name = "DeleteAudience")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(CreateProblemDetailsResponse("Invalid Id"));
            }

            _audienceService.DeleteById(id);
            Audience audience = await _audienceService.GetByIdAsync(id);
            return audience == null
                ? StatusCode(StatusCodes.Status500InternalServerError)
                : Ok();
        }
    }
}