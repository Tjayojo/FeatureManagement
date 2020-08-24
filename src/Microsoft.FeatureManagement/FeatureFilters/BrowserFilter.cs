using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.FeatureFilters.Settings;

namespace Microsoft.FeatureManagement.FeatureFilters
{
    /// <summary>
    /// 
    /// </summary>
    [FilterAlias(Alias)]
    public class BrowserFilter : IFeatureFilter
    {
        private const string Alias = "Browser";

        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public BrowserFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        /// <inheritdoc />
        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            var settings = (BrowserFilterSettings) context.Parameters;
            string userAgent = GetUserAgent();

            if (settings.AllowedBrowsers.Any(browser => browser.Equals(SupportedBrowserId.Chrome.ToString(),
                StringComparison.OrdinalIgnoreCase)) && IsChrome(userAgent))
            {
                return Task.FromResult(true);
            }

            if (settings.AllowedBrowsers.Any(browser => browser.Equals(SupportedBrowserId.Edge.ToString(),
                StringComparison.OrdinalIgnoreCase)) && IsEdge(userAgent))
            {
                return Task.FromResult(true);
            }

            if (settings.AllowedBrowsers.Any(browser => browser.Equals(SupportedBrowserId.InternetExplorer.ToString(),
                StringComparison.OrdinalIgnoreCase)) && IsInternetExplorer11(userAgent))
            {
                return Task.FromResult(true);
            }

            if (settings.AllowedBrowsers.Any(browser => browser.Equals(SupportedBrowserId.Firefox.ToString(),
                StringComparison.OrdinalIgnoreCase)) && IsFirefox(userAgent))
            {
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        private StringValues GetUserAgent() => _httpContextAccessor.HttpContext.Request.Headers["User-Agent"];

        private static bool IsChrome(string userAgent)
        {
            return !string.IsNullOrWhiteSpace(userAgent)
                   && userAgent.ToLowerInvariant().Contains("chrome")
                   && !userAgent.ToLowerInvariant().Contains("edge");
        }

        private static bool IsEdge(string userAgent)
        {
            return !string.IsNullOrWhiteSpace(userAgent)
                   && !userAgent.ToLowerInvariant().Contains("chrome")
                   && userAgent.ToLowerInvariant().Contains("edge");
        }

        private static bool IsInternetExplorer11(string userAgent)
        {
            return !string.IsNullOrWhiteSpace(userAgent)
                   && userAgent.ToLowerInvariant().Contains("trident");
        }

        private static bool IsFirefox(string userAgent)
        {
            return !string.IsNullOrWhiteSpace(userAgent)
                   && userAgent.ToLowerInvariant().Contains("firefox");
        }
    }
}