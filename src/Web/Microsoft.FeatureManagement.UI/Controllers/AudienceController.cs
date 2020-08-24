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
    public class AudienceController : BaseController
    {
        /// <inheritdoc />
        public AudienceController(IOptions<ApiOptions> apiOptions) : base(apiOptions)
        {
        }

        /// <summary>
        /// Get all audiences
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetAudience")]
        [ProducesResponseType(typeof(List<Audience>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            using (IFeatureManagementAppTierAPI api = CreateFeatureManagementApi())
            {
                HttpOperationResponse<IList<Audience>> result = await api.GetAllAudiencesWithHttpMessagesAsync();
                return CreateResponse(result);
            }
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
                return BadRequest("Invalid Id");
            }

            using IFeatureManagementAppTierAPI api = CreateFeatureManagementApi();
            HttpOperationResponse<object> result = await api.GetAudienceByIdWithHttpMessagesAsync(id);
            return CreateResponse<Audience>(result);
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
                return BadRequest("Invalid Id");
            }

            using (IFeatureManagementAppTierAPI api = CreateFeatureManagementApi())
            {
                HttpOperationResponse<object> result =
                    await api.GetAudienceByFeatureIdWithHttpMessagesAsync(featureId);
                return CreateResponse<Audience>(result);
            }
        }

        /// <summary>
        /// Create a new audience
        /// </summary>
        /// <param name="audience"></param>
        /// <returns></returns>
        [HttpPost(Name = "PostAudience")]
        [ProducesResponseType(typeof(Audience), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(Audience audience)
        {
            if (audience == null)
            {
                return BadRequest("Audience is required");
            }

            using (IFeatureManagementAppTierAPI api = CreateFeatureManagementApi())
            {
                HttpOperationResponse<object> result = await api.PostAudienceWithHttpMessagesAsync(audience);
                return CreateResponse<Audience>(result);
            }
        }

        /// <summary>
        /// Update an audience
        /// </summary>
        /// <param name="id"></param>
        /// <param name="audience"></param>
        /// <returns></returns>
        [HttpPut("{id}", Name = "PutAudience")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Audience), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put(Guid id, Audience audience)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid Id");
            }

            if (audience == null)
            {
                return BadRequest("Audience is required");
            }

            using (IFeatureManagementAppTierAPI api = CreateFeatureManagementApi())
            {
                HttpOperationResponse<object> result = await api.PutAudienceWithHttpMessagesAsync(id, audience);
                return CreateResponse<Audience>(result);
            }
        }

        /// <summary>
        /// Delete a audience
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete(Name = "DeleteAudience")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid Id");
            }

            using (IFeatureManagementAppTierAPI api = CreateFeatureManagementApi())
            {
                HttpOperationResponse result = await api.DeleteAudienceWithHttpMessagesAsync(id);
                return CreateResponse(result);
            }
        }
    }
}