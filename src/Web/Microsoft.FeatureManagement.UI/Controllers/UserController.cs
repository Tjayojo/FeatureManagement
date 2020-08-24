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
    public class UserController : BaseController
    {
        /// <inheritdoc />
        public UserController(IOptions<ApiOptions> apiOptions) : base(apiOptions)
        {
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetUser")]
        [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            using (IFeatureManagementAppTierAPI api = CreateFeatureManagementApi())
            {
                HttpOperationResponse<IList<User>> result = await api.GetAllUsersWithHttpMessagesAsync();
                return CreateResponse(result);
            }
        }

        /// <summary>
        /// Get user by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetUserById")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid Id");
            }

            using IFeatureManagementAppTierAPI api = CreateFeatureManagementApi();
            HttpOperationResponse<object> result = await api.GetUserByIdWithHttpMessagesAsync(id);
            return CreateResponse<User>(result);
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost(Name = "PostUser")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(User user)
        {
            if (user == null)
            {
                return BadRequest("User is required");
            }

            using (IFeatureManagementAppTierAPI api = CreateFeatureManagementApi())
            {
                HttpOperationResponse<object> result = await api.PostUserWithHttpMessagesAsync(user);
                return CreateResponse<User>(result);
            }
        }

        /// <summary>
        /// Update an user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("{id}", Name = "PutUser")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put(Guid id, User user)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid Id");
            }

            if (user == null)
            {
                return BadRequest("User is required");
            }

            using (IFeatureManagementAppTierAPI api = CreateFeatureManagementApi())
            {
                HttpOperationResponse<object> result = await api.PutUserWithHttpMessagesAsync(id, user);
                return CreateResponse<User>(result);
            }
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete(Name = "DeleteUser")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid Id");
            }

            using (IFeatureManagementAppTierAPI api = CreateFeatureManagementApi())
            {
                HttpOperationResponse result = await api.DeleteUserWithHttpMessagesAsync(id);
                return CreateResponse(result);
            }
        }
    }
}