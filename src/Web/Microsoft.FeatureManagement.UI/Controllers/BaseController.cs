using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement.Client;
using Microsoft.Rest;

namespace Microsoft.FeatureManagement.UI.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly ApiOptions _apiOptions;

        public BaseController(IOptions<ApiOptions> apiOptions)
        {
            _apiOptions = apiOptions?.Value ?? throw new ArgumentNullException(nameof(apiOptions));
        }

        internal IFeatureManagementAppTierAPI CreateFeatureManagementApi()
        {
            //Todo: Wire up service to get bearer tokens
            var credentials = new TokenCredentials("Bearer token here");
            var baseUri = new Uri(_apiOptions.FeatureManagementUri, UriKind.Absolute);
            return new FeatureManagementAppTierAPI(baseUri, credentials);
        }

        internal IActionResult CreateResponse<TResponse>(HttpOperationResponse<object> httpOperationResponse)
            where TResponse : class
        {
            int statusCode = (int) httpOperationResponse.Response.StatusCode;
            if (httpOperationResponse.Body == null)
            {
                return StatusCode(statusCode);
            }

            //Todo : Test the cast
            return StatusCode(statusCode,
                httpOperationResponse.Body is TResponse
                    ? httpOperationResponse.Body as TResponse
                    : httpOperationResponse.Body);
        }

        internal IActionResult CreateResponse<TResponse>(HttpOperationResponse<TResponse> httpOperationResponse)
            where TResponse : class
        {
            int statusCode = (int) httpOperationResponse.Response.StatusCode;

            if (httpOperationResponse.Body == null)
            {
                return StatusCode(statusCode);
            }

            return StatusCode(statusCode, httpOperationResponse.Body);
        }

        internal IActionResult CreateResponse(HttpOperationResponse httpOperationResponse) =>
            StatusCode((int) httpOperationResponse.Response.StatusCode);
    }
}