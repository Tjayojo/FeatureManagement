using System.Collections.Generic;

namespace Microsoft.FeatureManagement.FeatureFilters.Settings
{
    /// <inheritdoc />
    public class BrowserFilterSettings : IFilterSettings
    {
        /// <summary>
        /// Collection of allowed browser names
        /// </summary>
        public IList<string> AllowedBrowsers { get; set; } = new List<string>();
    }
}