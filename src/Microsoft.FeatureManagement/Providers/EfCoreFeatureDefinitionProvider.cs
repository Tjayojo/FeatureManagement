using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.FeatureFilters.Settings;
using Microsoft.FeatureManagement.Service.Interfaces;
using Audience = Microsoft.FeatureManagement.FeatureFilters.Audience;
using GroupRollout = Microsoft.FeatureManagement.FeatureFilters.GroupRollout;

namespace Microsoft.FeatureManagement.Providers
{
    internal sealed class EfCoreFeatureDefinitionProvider : IFeatureDefinitionProvider
    {
        private readonly IFeatureService _featureService;
        private readonly ConcurrentDictionary<string, FeatureDefinition> _definitions;

        public EfCoreFeatureDefinitionProvider(IFeatureService featureService)
        {
            _featureService = featureService ?? throw new ArgumentNullException(nameof(featureService));
            _definitions = new ConcurrentDictionary<string, FeatureDefinition>();
        }

        /// <inheritdoc />
        public async Task<FeatureDefinition> GetFeatureDefinitionAsync(string featureName,
            CancellationToken cancellationToken = default)
        {
            if (featureName == null)
            {
                throw new ArgumentNullException(nameof(featureName));
            }

            Feature feature = await _featureService.GetByName(featureName, cancellationToken)
                .ConfigureAwait(false);

            if (feature == null)
            {
                throw new FeatureManagementException(FeatureManagementError.MissingFeature,
                    $"Feature {featureName} not found");
            }

            FeatureDefinition definition = _definitions.GetOrAdd(featureName, _ => ReadFeatureDefinition(feature));

            return definition;
        }

        /// <inheritdoc />
        public async IAsyncEnumerable<FeatureDefinition> GetAllFeatureDefinitionsAsync(
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            IList<Feature> features = await _featureService.GetAllAsync()
                .ConfigureAwait(false);

            foreach (Feature feature in features)
            {
                yield return _definitions.GetOrAdd(feature.Name, _ => ReadFeatureDefinition(feature));
            }
        }

        private static FeatureDefinition ReadFeatureDefinition(Feature feature)
        {
            var enabledFor = new List<FeatureFilterConfiguration>();
            if (feature.AlwaysOn)
            {
                enabledFor.Add(new FeatureFilterConfiguration
                {
                    Name = "AlwaysOn"
                });
            }
            else if (feature.AlwaysOff || !feature.IsEnabled)
            {
                enabledFor.Add(new FeatureFilterConfiguration
                {
                    Name = "AlwaysOff"
                });
            }
            else
            {
                if (feature.TimeWindow?.IsActive == true)
                {
                    enabledFor.Add(new FeatureFilterConfiguration
                    {
                        Name = "TimeWindow",
                        Parameters = new TimeWindowFilterSettings
                        {
                            Start = feature.TimeWindow.Start,
                            End = feature.TimeWindow.End
                        }
                    });
                }

                if (feature.RolloutPercentage?.IsActive == true)
                {
                    enabledFor.Add(new FeatureFilterConfiguration
                    {
                        Name = "Percentage",
                        Parameters = new PercentageFilterSettings
                        {
                            Value = feature.RolloutPercentage.Percentage
                        }
                    });
                }

                //Aka Targeting
                if (feature.Audience?.IsActive == true)
                {
                    enabledFor.Add(new FeatureFilterConfiguration
                    {
                        Name = "Targeting",
                        Parameters = new TargetingFilterSettings
                        {
                            Audience = new Audience
                            {
                                Users = feature.Audience.Users?.Select(u => u.UserName).ToList(),
                                Groups = feature.Audience.GroupRollouts?
                                    .Select(g => new GroupRollout
                                    {
                                        Name = g.Name,
                                        RolloutPercentage = g.RolloutPercentage
                                    }).ToList(),
                                DefaultRolloutPercentage = feature.Audience.DefaultRolloutPercentage
                            }
                        }
                    });
                }

                if (feature.BrowserRestrictions.Any(br => br.IsActive))
                {
                    enabledFor.Add(new FeatureFilterConfiguration
                    {
                        Name = "Browser",
                        Parameters = new BrowserFilterSettings
                        {
                            AllowedBrowsers = feature.BrowserRestrictions
                                .Where(b => b.IsActive)
                                .Select(b => b.SupportedBrowserId.ToString())
                                .ToList()
                        }
                    });
                }
            }

            return new FeatureDefinition
            {
                Name = feature.Name,
                EnabledFor = enabledFor
            };
        }
    }
}