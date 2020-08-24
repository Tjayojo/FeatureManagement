using Microsoft.FeatureManagement.Targeting;

namespace Microsoft.FeatureManagement.FeatureFilters.Settings
{
    /// <summary>
    /// The settings that are used to configure the <see cref="TargetingFilter"/> feature filter.
    /// </summary>
    public class TargetingFilterSettings : IFilterSettings
    {
        /// <summary>
        /// The audience that a feature configured to use the <see cref="TargetingFilter"/> should be enabled for.
        /// </summary>
        public Audience Audience { get; set; }
    }
}