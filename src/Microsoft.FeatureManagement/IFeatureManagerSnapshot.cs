// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//

using Microsoft.FeatureManagement.Managers;

namespace Microsoft.FeatureManagement
{
    /// <summary>
    /// Provides a snapshot of feature state to ensure consistency across a given request.
    /// </summary>
    public interface IFeatureManagerSnapshot : IFeatureManager
    {
    }
}
