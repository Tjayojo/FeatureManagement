using System.Collections.Generic;

namespace Microsoft.FeatureManagement.FeatureFilters.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountFilterSettings : IFilterSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public IList<string> AllowedAccounts { get; set; } = new List<string>();
    }
}