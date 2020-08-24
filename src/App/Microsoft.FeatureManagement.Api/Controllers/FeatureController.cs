using System;
using System.Collections.Generic;
using System.Linq;
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
    public class FeatureController : BaseController
    {
        private readonly IFeatureService _featureService;
        private readonly ISupportedBrowserService _supportedBrowserService;
        private readonly IBrowserRestrictionService _browserRestrictionService;

        public FeatureController(IFeatureService featureService, ISupportedBrowserService supportedBrowserService,
            IBrowserRestrictionService browserRestrictionService)
        {
            _featureService = featureService ?? throw new ArgumentNullException(nameof(featureService));
            _supportedBrowserService = supportedBrowserService ??
                                       throw new ArgumentNullException(nameof(supportedBrowserService));
            _browserRestrictionService = browserRestrictionService ??
                                         throw new ArgumentNullException(nameof(browserRestrictionService));
        }

        /// <summary>
        /// Get all features
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetAllFeatures")]
        [ProducesResponseType(typeof(IList<Feature>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            IList<Feature> features = await _featureService
                .GetAllAsync()
                .ConfigureAwait(false);

            return Ok(features);
        }

        /// <summary>
        /// Get a feature by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("[action]/{name}", Name = "GetFeatureByName")]
        [ProducesResponseType(typeof(Feature), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest(CreateProblemDetailsResponse("Invalid feature name provided"));
            }

            Feature feature = await _featureService.GetByName(name).ConfigureAwait(false);
            return feature == null ? (IActionResult) NotFound() : Ok(feature);
        }

        /// <summary>
        /// Get feature by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetFeatureById")]
        [ProducesResponseType(typeof(Feature), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(CreateProblemDetailsResponse("Invalid Id"));
            }

            Feature feature = await _featureService
                .GetByIdAsync(id)
                .ConfigureAwait(false);

            return feature == null
                ? (IActionResult) NotFound()
                : Ok(feature);
        }

        /// <summary>
        /// Create a new feature
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        [HttpPost(Name = "PostFeature")]
        [ProducesResponseType(typeof(Feature), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(Feature feature)
        {
            if (feature == null)
            {
                return BadRequest(CreateProblemDetailsResponse("Feature is required"));
            }

            if (string.IsNullOrWhiteSpace(feature.Name))
            {
                return BadRequest(CreateProblemDetailsResponse("Invalid feature name"));
            }

            Feature existingFeatureWithName = await _featureService
                .GetByName(feature.Name)
                .ConfigureAwait(false);

            if (existingFeatureWithName != null)
            {
                return BadRequest(CreateProblemDetailsResponse("Feature with the same name already exists"));
            }

            feature.IsEnabled = true;
            feature.CreatedOn = DateTimeOffset.Now;
            Feature newFeature = await _featureService
                .AddAsync(feature)
                .ConfigureAwait(false);

            if (newFeature == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            if (feature.BrowserRestrictions != null && !feature.BrowserRestrictions.Any())
            {
                return Ok(newFeature);
            }

            IList<SupportedBrowser> supportedBrowsers = await _supportedBrowserService.GetAllAsync()
                .ConfigureAwait(false);

            foreach (SupportedBrowser supportedBrowser in supportedBrowsers)
            {
                newFeature.BrowserRestrictions.ToList()
                    .Add(
                        await _browserRestrictionService.AddAsync(
                            new BrowserRestriction
                            {
                                // CreatedBy = ,
                                CreatedOn = DateTimeOffset.Now,
                                FeatureId = newFeature.Id,
                                IsActive = false,
                                SupportedBrowserId = supportedBrowser.SupportedBrowserId
                            }).ConfigureAwait(false));
            }

            return Ok(newFeature);
        }

        /// <summary>
        /// Update a feature
        /// </summary>
        /// <param name="id"></param>
        /// <param name="feature"></param>
        /// <returns></returns>
        [HttpPut("{id}", Name = "PutFeature")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Feature), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(Guid id, Feature feature)
        {
            #region Validation

            if (id == Guid.Empty)
            {
                return BadRequest(CreateProblemDetailsResponse("Invalid Id"));
            }

            if (feature == null)
            {
                return BadRequest(CreateProblemDetailsResponse("Feature is required"));
            }

            if (string.IsNullOrWhiteSpace(feature.Name))
            {
                return BadRequest(CreateProblemDetailsResponse("Invalid Feature Name"));
            }

            #endregion

            Feature existingFeature = await _featureService
                .GetByIdWithNoTrackingAsync(id)
                .ConfigureAwait(false);

            if (existingFeature == null)
            {
                return NotFound();
            }

            if (id != feature.Id || existingFeature.Id != feature.Id)
            {
                return BadRequest(CreateProblemDetailsResponse("Feature Id mismatch"));
            }

            feature.ModifiedOn = DateTimeOffset.Now;
            Feature updatedFeature = _featureService.Update(feature);
            return updatedFeature == null
                ? (IActionResult) StatusCode(StatusCodes.Status500InternalServerError)
                : Ok(updatedFeature);
        }

        /// <summary>
        /// Delete a feature
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete(Name = "DeleteFeature")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(CreateProblemDetailsResponse("Invalid Id"));
            }

            _featureService.DeleteById(id);
            Feature feature = await _featureService.GetByIdAsync(id);
            return feature == null
                ? StatusCode(StatusCodes.Status500InternalServerError)
                : Ok();
        }
    }
}