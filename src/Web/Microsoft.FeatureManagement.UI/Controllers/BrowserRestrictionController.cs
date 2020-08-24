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
    public class BrowserRestrictionController : BaseController
    {
        /// <inheritdoc />
        public BrowserRestrictionController(IOptions<ApiOptions> apiOptions) : base(apiOptions)
        {
        }

        /// <summary>
        /// Get all browser restrictions
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetBrowserRestriction")]
        [ProducesResponseType(typeof(List<BrowserRestriction>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            using (IFeatureManagementAppTierAPI api = CreateFeatureManagementApi())
            {
                HttpOperationResponse<IList<BrowserRestriction>> result =
                    await api.GetAllBrowserRestrictionsWithHttpMessagesAsync();
                return CreateResponse(result);
            }
        }

        /// <summary>
        /// Get browser restriction by Id
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
                return BadRequest("Invalid Id");
            }

            using IFeatureManagementAppTierAPI api = CreateFeatureManagementApi();
            HttpOperationResponse<object> result = await api.GetBrowserRestrictionByIdWithHttpMessagesAsync(id);
            return CreateResponse<BrowserRestriction>(result);
        }

        /// <summary>
        /// Get browser restriction by Feature Id
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
                return BadRequest("Invalid Id");
            }

            using (IFeatureManagementAppTierAPI api = CreateFeatureManagementApi())
            {
                HttpOperationResponse<object> result =
                    await api.GetBrowserRestrictionByFeatureIdWithHttpMessagesAsync(featureId);
                return CreateResponse<BrowserRestriction>(result);
            }
        }

        /// <summary>
        /// Create a new browser restriction
        /// </summary>
        /// <param name="browserRestriction"></param>
        /// <returns></returns>
        [HttpPost(Name = "PostBrowserRestriction")]
        [ProducesResponseType(typeof(BrowserRestriction), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(BrowserRestriction browserRestriction)
        {
            if (browserRestriction == null)
            {
                return BadRequest("BrowserRestriction is required");
            }

            using (IFeatureManagementAppTierAPI api = CreateFeatureManagementApi())
            {
                HttpOperationResponse<object> result =
                    await api.PostBrowserRestrictionWithHttpMessagesAsync(browserRestriction);
                return CreateResponse<BrowserRestriction>(result);
            }
        }

        /// <summary>
        /// Update an browser restriction
        /// </summary>
        /// <param name="id"></param>
        /// <param name="browserRestriction"></param>
        /// <returns></returns>
        [HttpPut("{id}", Name = "PutBrowserRestriction")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BrowserRestriction), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put(Guid id, BrowserRestriction browserRestriction)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid Id");
            }

            if (browserRestriction == null)
            {
                return BadRequest("BrowserRestriction is required");
            }

            using (IFeatureManagementAppTierAPI api = CreateFeatureManagementApi())
            {
                HttpOperationResponse<object> result =
                    await api.PutBrowserRestrictionWithHttpMessagesAsync(id, browserRestriction);
                return CreateResponse<BrowserRestriction>(result);
            }
        }

        /// <summary>
        /// Delete a browser restriction
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete(Name = "DeleteBrowserRestriction")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid Id");
            }

            using (IFeatureManagementAppTierAPI api = CreateFeatureManagementApi())
            {
                HttpOperationResponse result = await api.DeleteBrowserRestrictionWithHttpMessagesAsync(id);
                return CreateResponse(result);
            }
        }
    }
}