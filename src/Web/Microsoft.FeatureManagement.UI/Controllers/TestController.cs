using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement.Managers;

namespace Microsoft.FeatureManagement.UI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TestController : BaseController
    {
        private readonly IFeatureManager _featureManager;
        private readonly ILogger<TestController> _logger;

        /// <inheritdoc />
        public TestController(IOptions<ApiOptions> apiOptions, IFeatureManager featureManager,
            ILoggerFactory loggerFactory) :
            base(apiOptions)
        {
            _featureManager =
                featureManager ?? throw new ArgumentNullException(nameof(featureManager));
            _logger = loggerFactory?.CreateLogger<TestController>() ??
                      throw new ArgumentNullException(nameof(loggerFactory));
        }

        [HttpGet("{featureName}", Name = "TestFeature")]
        public async Task<IActionResult> Get(string featureName)
        {
            if (string.IsNullOrWhiteSpace(featureName))
            {
                return BadRequest("Invalid feature name");
            }

            try
            {
                bool result = await _featureManager.IsEnabledAsync(featureName);
                return Ok(result);
            }
            catch (FeatureManagementException featureManagementException)
            {
                _logger.LogWarning(featureManagementException.Message, featureManagementException);
                return BadRequest(featureManagementException.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return BadRequest("Internal Server Error");
            }
        }
    }
}