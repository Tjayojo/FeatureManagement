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
    public class GroupRolloutController : BaseController
    {
        /// <inheritdoc />
        public GroupRolloutController(IOptions<ApiOptions> apiOptions) : base(apiOptions)
        {
        }

        /// <summary>
        /// Get all group rollouts
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetGroupRollout")]
        [ProducesResponseType(typeof(List<GroupRollout>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            using (IFeatureManagementAppTierAPI api = CreateFeatureManagementApi())
            {
                HttpOperationResponse<IList<GroupRollout>> result = await api.GetAllGroupRolloutsWithHttpMessagesAsync();
                return CreateResponse(result);
            }
        }

        /// <summary>
        /// Get group rollout by Id
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
                return BadRequest("Invalid Id");
            }

            using IFeatureManagementAppTierAPI api = CreateFeatureManagementApi();
            HttpOperationResponse<object> result = await api.GetGroupRolloutByIdWithHttpMessagesAsync(id);
            return CreateResponse<GroupRollout>(result);
        }

        /// <summary>
        /// Create a new group rollout
        /// </summary>
        /// <param name="groupRollout"></param>
        /// <returns></returns>
        [HttpPost(Name = "PostGroupRollout")]
        [ProducesResponseType(typeof(GroupRollout), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(GroupRollout groupRollout)
        {
            if (groupRollout == null)
            {
                return BadRequest("GroupRollout is required");
            }

            using (IFeatureManagementAppTierAPI api = CreateFeatureManagementApi())
            {
                HttpOperationResponse<object> result = await api.PostGroupRolloutWithHttpMessagesAsync(groupRollout);
                return CreateResponse<GroupRollout>(result);
            }
        }

        /// <summary>
        /// Update an group rollout
        /// </summary>
        /// <param name="id"></param>
        /// <param name="groupRollout"></param>
        /// <returns></returns>
        [HttpPut("{id}", Name = "PutGroupRollout")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GroupRollout), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put(Guid id, GroupRollout groupRollout)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid Id");
            }

            if (groupRollout == null)
            {
                return BadRequest("GroupRollout is required");
            }

            using (IFeatureManagementAppTierAPI api = CreateFeatureManagementApi())
            {
                HttpOperationResponse<object> result = await api.PutGroupRolloutWithHttpMessagesAsync(id, groupRollout);
                return CreateResponse<GroupRollout>(result);
            }
        }

        /// <summary>
        /// Delete a group rollout
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete(Name = "DeleteGroupRollout")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid Id");
            }

            using (IFeatureManagementAppTierAPI api = CreateFeatureManagementApi())
            {
                HttpOperationResponse result = await api.DeleteGroupRolloutWithHttpMessagesAsync(id);
                return CreateResponse(result);
            }
        }
    }
}