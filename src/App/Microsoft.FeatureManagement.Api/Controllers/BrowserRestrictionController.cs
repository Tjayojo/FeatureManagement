using System;
using System.Collections.Generic;
using System.Linq;
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
    public class BrowserRestrictionController : BaseController
    {
        private readonly IBrowserRestrictionService _browserRestrictionService;
        private readonly IFeatureService _featureService;

        public BrowserRestrictionController(IBrowserRestrictionService browserRestrictionService,
            IFeatureService featureService)
        {
            _browserRestrictionService = browserRestrictionService ??
                                         throw new ArgumentNullException(nameof(browserRestrictionService));
            _featureService = featureService ?? throw new ArgumentNullException(nameof(featureService));
        }

        /// <summary>
        /// Get all Browser Restrictions
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetAllBrowserRestrictions")]
        [ProducesResponseType(typeof(IList<BrowserRestriction>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            IList<BrowserRestriction> browserRestrictions = await _browserRestrictionService
                .GetAllAsync()
                .ConfigureAwait(false);

            return Ok(browserRestrictions);
        }

        /// <summary>
        /// Get Browser Restriction by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetBrowserRestrictionById")]
        [ProducesResponseType(typeof(BrowserRestriction), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(CreateProblemDetailsResponse("Invalid Id"));
            }

            BrowserRestriction browserRestriction = await _browserRestrictionService
                .GetByIdAsync(id)
                .ConfigureAwait(false);

            return browserRestriction == null
                ? (IActionResult) NotFound()
                : Ok(browserRestriction);
        }

        /// <summary>
        /// Get Browser Restriction by feature Id
        /// </summary>
        /// <param name="featureId"></param>
        /// <returns></returns>
        [HttpGet("[action]/{featureId}", Name = "GetBrowserRestrictionByFeatureId")]
        [ProducesResponseType(typeof(BrowserRestriction), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByFeatureId(Guid featureId)
        {
            if (featureId == Guid.Empty)
            {
                return BadRequest(CreateProblemDetailsResponse("Invalid Id"));
            }

            List<BrowserRestriction> browserRestriction = await _browserRestrictionService
                .GetByFeatureId(featureId)
                .ConfigureAwait(false);

            return Ok(browserRestriction);
        }

        /// <summary>
        /// Create a new Browser Restriction
        /// </summary>
        /// <param name="browserRestriction"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost(Name = "PostBrowserRestriction")]
        [ProducesResponseType(typeof(BrowserRestriction), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(BrowserRestriction browserRestriction,
            CancellationToken cancellationToken = default)
        {
            if (browserRestriction == null)
            {
                return BadRequest(CreateProblemDetailsResponse("BrowserRestriction is required"));
            }

            Feature feature = await _featureService
                .GetByIdAsync(browserRestriction.FeatureId)
                .ConfigureAwait(false);

            if (feature == null)
            {
                return BadRequest(CreateProblemDetailsResponse("Feature not found"));
            }

            List<BrowserRestriction> featureBrowserRestrictions = await _browserRestrictionService
                .GetByFeatureId(browserRestriction.FeatureId, cancellationToken).ConfigureAwait(false);

            if (featureBrowserRestrictions.Any(f => f.SupportedBrowserId == browserRestriction.SupportedBrowserId))
            {
                ProblemDetails problemDetailsResponse = CreateProblemDetailsResponse(
                    $"Feature already has a browser restriction for {browserRestriction.SupportedBrowserId.ToString()}");
                return BadRequest(problemDetailsResponse);
            }

            browserRestriction.CreatedOn = DateTimeOffset.Now;
            BrowserRestriction newBrowserRestriction = await _browserRestrictionService
                .AddAsync(browserRestriction)
                .ConfigureAwait(false);

            return newBrowserRestriction == null
                ? (IActionResult) StatusCode(StatusCodes.Status500InternalServerError)
                : Ok(newBrowserRestriction);
        }

        /// <summary>
        /// Update a Browser Restriction
        /// </summary>
        /// <param name="id"></param>
        /// <param name="browserRestriction"></param>
        /// <returns></returns>
        [HttpPut("{id}", Name = "PutBrowserRestriction")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(BrowserRestriction), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put(Guid id, BrowserRestriction browserRestriction)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(CreateProblemDetailsResponse("Invalid Id"));
            }

            if (browserRestriction == null)
            {
                return BadRequest(CreateProblemDetailsResponse("BrowserRestriction is required"));
            }

            BrowserRestriction existingBrowserRestriction = await _browserRestrictionService
                .GetByIdAsync(id)
                .ConfigureAwait(false);

            if (existingBrowserRestriction == null)
            {
                return NotFound();
            }

            if (existingBrowserRestriction.Id != browserRestriction.Id)
            {
                return BadRequest(CreateProblemDetailsResponse("BrowserRestriction Id mismatch"));
            }

            Feature feature = await _featureService
                .GetByIdAsync(browserRestriction.FeatureId)
                .ConfigureAwait(false);

            if (feature == null)
            {
                return BadRequest(CreateProblemDetailsResponse("Feature not found"));
            }

            browserRestriction.ModifiedOn = DateTimeOffset.Now;
            BrowserRestriction updatedBrowserRestriction = _browserRestrictionService.Update(browserRestriction);
            return updatedBrowserRestriction == null
                ? (IActionResult) StatusCode(StatusCodes.Status500InternalServerError)
                : Ok(updatedBrowserRestriction);
        }

        /// <summary>
        /// Delete a Browser Restriction
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete(Name = "DeleteBrowserRestriction")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(CreateProblemDetailsResponse("Invalid Id"));
            }

            BrowserRestriction existingBrowserRestriction = await _browserRestrictionService.GetByIdAsync(id)
                .ConfigureAwait(false);
            if (existingBrowserRestriction == null)
            {
                return NotFound();
            }

            _browserRestrictionService.DeleteById(id);
            BrowserRestriction browserRestriction = await _browserRestrictionService.GetByIdAsync(id);
            return browserRestriction == null
                ? StatusCode(StatusCodes.Status500InternalServerError)
                : Ok();
        }
    }
}