// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//

using Microsoft.FeatureManagement.FeatureFilters;

namespace Microsoft.FeatureManagement
{
    /// <summary>
    /// The configuration of a feature filter.
    /// </summary>
    public class FeatureFilterConfiguration
    {
        /// <summary>
        /// The name of the feature filter.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Configurable parameters that can change across instances of a feature filter.
        /// </summary>
        public IFilterSettings Parameters { get; set; }
    }
}
