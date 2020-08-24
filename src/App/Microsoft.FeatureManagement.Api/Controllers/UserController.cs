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
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetAllUsers")]
        [ProducesResponseType(typeof(IList<User>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            IList<User> users = await _userService
                .GetAllAsync()
                .ConfigureAwait(false);

            return Ok(users);
        }

        /// <summary>
        /// Get a user by UserName
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet("[action]/{userName}", Name = "GetUserByUsername")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest(CreateProblemDetailsResponse("Invalid UserName provided"));
            }

            User user = await _userService
                .GetByUserName(userName)
                .ConfigureAwait(false);
            return user == null ? (IActionResult) NotFound() : Ok(user);
        }

        /// <summary>
        /// Get user by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetUserById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(CreateProblemDetailsResponse("Invalid Id"));
            }

            User user = await _userService
                .GetByIdAsync(id)
                .ConfigureAwait(false);

            return user == null
                ? (IActionResult) NotFound()
                : Ok(user);
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost(Name = "PostUser")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(User user)
        {
            if (user == null)
            {
                return BadRequest(CreateProblemDetailsResponse("User is required"));
            }

            User existingUserWithName = await _userService
                .GetByUserName(user.UserName)
                .ConfigureAwait(false);

            if (existingUserWithName != null)
            {
                return BadRequest(CreateProblemDetailsResponse("User with the same name already exists"));
            }
            
            user.CreatedOn = DateTimeOffset.Now;

            User newUser = await _userService
                .AddAsync(user)
                .ConfigureAwait(false);

            return newUser == null
                ? (IActionResult) StatusCode(StatusCodes.Status500InternalServerError)
                : Ok(newUser);
        }

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("{id}", Name = "PutUser")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(Guid id, User user)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(CreateProblemDetailsResponse("Invalid Id"));
            }

            if (user == null)
            {
                return BadRequest(CreateProblemDetailsResponse("User is required"));
            }

            User existingUser = await _userService
                .GetByIdAsync(id)
                .ConfigureAwait(false);

            if (existingUser == null)
            {
                return NotFound();
            }

            if (existingUser.Id != user.Id)
            {
                return BadRequest(CreateProblemDetailsResponse("User Id mismatch"));
            }

            user.ModifiedOn = DateTimeOffset.Now;
            User updatedUser = _userService.Update(user);
            return updatedUser == null
                ? (IActionResult) StatusCode(StatusCodes.Status500InternalServerError)
                : Ok(updatedUser);
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete(Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(CreateProblemDetailsResponse("Invalid Id"));
            }

            _userService.DeleteById(id);
            User user = await _userService.GetByIdAsync(id);
            return user == null
                ? StatusCode(StatusCodes.Status500InternalServerError)
                : Ok();
        }
    }
}