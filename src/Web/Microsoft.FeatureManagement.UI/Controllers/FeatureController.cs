using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement.Client;
using Microsoft.FeatureManagement.Client.Models;
using Microsoft.Rest;

namespace Microsoft.FeatureManagement.UI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class FeatureController : BaseController
    {
        /// <inheritdoc />
        public FeatureController(IOptions<ApiOptions> apiOptions) : base(apiOptions)
        {
        }

        /// <summary>
        /// Get all features
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetFeature")]
        [ProducesResponseType(typeof(List<Feature>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            using (IFeatureManagementAppTierAPI api = CreateFeatureManagementApi())
            {
                HttpOperationResponse<IList<Feature>> result = await api.GetAllFeaturesWithHttpMessagesAsync();
                return CreateResponse(result);
            }
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
                return BadRequest("Invalid Id");
            }

            using IFeatureManagementAppTierAPI api = CreateFeatureManagementApi();
            HttpOperationResponse<object> result = await api.GetFeatureByIdWithHttpMessagesAsync(id);
            return CreateResponse<Feature>(result);
        }

        /// <summary>
        /// Create a new feature
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        [HttpPost(Name = "PostFeature")]
        [ProducesResponseType(typeof(Feature), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(Feature feature)
        {
            if (feature == null)
            {
                return BadRequest("Feature is required");
            }

            if (string.IsNullOrWhiteSpace(feature.Name))
            {
                return BadRequest("Invalid Feature name");
            }

            using (IFeatureManagementAppTierAPI api = CreateFeatureManagementApi())
            {
                HttpOperationResponse<object> result = await api.PostFeatureWithHttpMessagesAsync(feature);
                return CreateResponse<Feature>(result);
            }
        }

        /// <summary>
        /// Update an feature
        /// </summary>
        /// <param name="id"></param>
        /// <param name="feature"></param>
        /// <returns></returns>
        [HttpPut("{id}", Name = "PutFeature")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Feature), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put(Guid id, Feature feature)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid Id");
            }

            if (feature == null)
            {
                return BadRequest("Feature is required");
            }

            using (IFeatureManagementAppTierAPI api = CreateFeatureManagementApi())
            {
                HttpOperationResponse<object> result = await api.PutFeatureWithHttpMessagesAsync(id, feature);
                return CreateResponse<Feature>(result);
            }
        }

        /// <summary>
        /// Delete a feature
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete(Name = "DeleteFeature")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid Id");
            }

            using (IFeatureManagementAppTierAPI api = CreateFeatureManagementApi())
            {
                HttpOperationResponse result = await api.DeleteFeatureWithHttpMessagesAsync(id);
                return CreateResponse(result);
            }
        }
    }
}