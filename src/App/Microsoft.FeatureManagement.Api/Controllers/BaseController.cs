using Microsoft.AspNetCore.Mvc;

namespace Microsoft.FeatureManagement.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        internal static ProblemDetails CreateProblemDetailsResponse(string title, string detail = default)
        {
            return new ProblemDetails
            {
                Detail = detail,
                Title = title
            };
        }
    }
}