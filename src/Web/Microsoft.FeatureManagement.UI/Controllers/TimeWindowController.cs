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
    public class TimeWindowController : BaseController
    {
        /// <inheritdoc />
        public TimeWindowController(IOptions<ApiOptions> apiOptions) : base(apiOptions)
        {
        }

        /// <summary>
        /// Get all time windows
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetTimeWindow")]
        [ProducesResponseType(typeof(List<TimeWindow>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            using (IFeatureManagementAppTierAPI api = CreateFeatureManagementApi())
            {
                HttpOperationResponse<IList<TimeWindow>> result = await api.GetAllTimeWindowsWithHttpMessagesAsync();
                return CreateResponse(result);
            }
        }

        /// <summary>
        /// Get time window by Id
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
                return BadRequest("Invalid Id");
            }

            using IFeatureManagementAppTierAPI api = CreateFeatureManagementApi();
            HttpOperationResponse<object> result = await api.GetTimeWindowByIdWithHttpMessagesAsync(id);
            return CreateResponse<TimeWindow>(result);
        }

        /// <summary>
        /// Get time window by Feature Id
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
                return BadRequest("Invalid Id");
            }

            using (IFeatureManagementAppTierAPI api = CreateFeatureManagementApi())
            {
                HttpOperationResponse<object> result =
                    await api.GetTimeWindowByFeatureIdWithHttpMessagesAsync(featureId);
                return CreateResponse<TimeWindow>(result);
            }
        }

        /// <summary>
        /// Create a new time window
        /// </summary>
        /// <param name="timeWindow"></param>
        /// <returns></returns>
        [HttpPost(Name = "PostTimeWindow")]
        [ProducesResponseType(typeof(TimeWindow), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(TimeWindow timeWindow)
        {
            if (timeWindow == null)
            {
                return BadRequest("TimeWindow is required");
            }

            using (IFeatureManagementAppTierAPI api = CreateFeatureManagementApi())
            {
                HttpOperationResponse<object> result = await api.PostTimeWindowWithHttpMessagesAsync(timeWindow);
                return CreateResponse<TimeWindow>(result);
            }
        }

        /// <summary>
        /// Update an time window
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timeWindow"></param>
        /// <returns></returns>
        [HttpPut("{id}", Name = "PutTimeWindow")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(TimeWindow), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put(Guid id, TimeWindow timeWindow)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid Id");
            }

            if (timeWindow == null)
            {
                return BadRequest("TimeWindow is required");
            }

            using (IFeatureManagementAppTierAPI api = CreateFeatureManagementApi())
            {
                HttpOperationResponse<object> result = await api.PutTimeWindowWithHttpMessagesAsync(id, timeWindow);
                return CreateResponse<TimeWindow>(result);
            }
        }

        /// <summary>
        /// Delete a time window
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete(Name = "DeleteTimeWindow")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid Id");
            }

            using (IFeatureManagementAppTierAPI api = CreateFeatureManagementApi())
            {
                HttpOperationResponse result = await api.DeleteTimeWindowWithHttpMessagesAsync(id);
                return CreateResponse(result);
            }
        }
    }
}