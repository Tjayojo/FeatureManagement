using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Microsoft.FeatureManagement.Targeting
{
    /// <summary>
    /// Provides an implementation of <see cref="ITargetingContextAccessor"/> that creates a targeting context using info from the current HTTP request.
    /// </summary>
    public class HttpContextTargetingContextAccessor : ITargetingContextAccessor
    {
        private const string TargetingContextLookup = "HttpContextTargetingContextAccessor.TargetingContext";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextTargetingContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public ValueTask<TargetingContext> GetContextAsync()
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;

            //
            // Try cache lookup
            if (httpContext.Items.TryGetValue(TargetingContextLookup, out object value))
            {
                return new ValueTask<TargetingContext>((TargetingContext)value);
            }

            ClaimsPrincipal user = httpContext.User;

            List<string> groups = user.Claims.Where(claim => claim.Type == ClaimTypes.GroupName)
                .Select(claim => claim.Value).ToList();

            //
            // This application expects groups to be specified in the user's claims

            //
            // Build targeting context based off user info
            var targetingContext = new TargetingContext
            {
                UserId = user.Identity.Name,
                Groups = groups
            };

            //
            // Cache for subsequent lookup
            httpContext.Items[TargetingContextLookup] = targetingContext;

            return new ValueTask<TargetingContext>(targetingContext);
        }
    }
}