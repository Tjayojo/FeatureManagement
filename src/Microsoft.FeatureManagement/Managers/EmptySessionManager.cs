// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//

using System.Threading.Tasks;

namespace Microsoft.FeatureManagement.Managers
{
    /// <summary>
    /// Empty implementation of <see cref="ISessionManager"/>.
    /// </summary>
    public class EmptySessionManager : ISessionManager
    {
        /// <inheritdoc />
        public Task SetAsync(string featureName, bool enabled)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<bool?> GetAsync(string featureName)
        {
            return Task.FromResult((bool?)null);
        }
    }
}
