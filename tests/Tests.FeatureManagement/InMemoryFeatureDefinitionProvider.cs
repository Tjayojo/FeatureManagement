﻿using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.FeatureManagement.Providers;

namespace Tests.FeatureManagement
{
    internal class InMemoryFeatureDefinitionProvider : IFeatureDefinitionProvider
    {
        private IEnumerable<FeatureDefinition> _definitions;

        public InMemoryFeatureDefinitionProvider(IEnumerable<FeatureDefinition> featureDefinitions)
        {
            _definitions = featureDefinitions ?? throw new ArgumentNullException(nameof(featureDefinitions));
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async IAsyncEnumerable<FeatureDefinition> GetAllFeatureDefinitionsAsync(
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            foreach (FeatureDefinition definition in _definitions)
            {
                yield return definition;
            }
        }

        public Task<FeatureDefinition> GetFeatureDefinitionAsync(string featureName,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_definitions.FirstOrDefault(definitions => definitions.Name.Equals(featureName, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
