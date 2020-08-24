using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement.Client;
using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.FeatureFilters.Settings;
using Microsoft.Rest;
using Feature = Microsoft.FeatureManagement.Client.Models.Feature;

namespace Microsoft.FeatureManagement.Providers
{
    internal sealed class ApiFeatureDefinitionProvider : IFeatureDefinitionProvider
    {
        private readonly ApiOptions _apiOptions;
        private readonly ConcurrentDictionary<string, FeatureDefinition> _definitions;

        public ApiFeatureDefinitionProvider(IOptions<ApiOptions> apiOptions)
        {
            _apiOptions = apiOptions?.Value ?? throw new ArgumentNullException(nameof(apiOptions));
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

            IFeatureManagementAppTierAPI featureApi = CreateFeatureManagementApi();
            var feature = await featureApi.GetFeatureByNameAsync(featureName, cancellationToken)
                .ConfigureAwait(false) as Feature;

            if (feature == null)
            {
                throw new FeatureManagementException(FeatureManagementError.MissingFeature,
                    $"Feature {featureName} not found");
            }

            FeatureDefinition definition = _definitions?.GetOrAdd(featureName, _ => ReadFeatureDefinition(feature));

            return definition;
        }

        /// <inheritdoc />
        public async IAsyncEnumerable<FeatureDefinition> GetAllFeatureDefinitionsAsync(
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            IFeatureManagementAppTierAPI featureApi = CreateFeatureManagementApi();
            IList<Feature> features = await featureApi.GetAllFeaturesAsync(cancellationToken).ConfigureAwait(false);
            foreach (Feature feature in features)
            {
                yield return _definitions.GetOrAdd(feature.Name, _ => ReadFeatureDefinition(feature));
            }
        }

        private static FeatureDefinition ReadFeatureDefinition(Feature feature)
        {
            var enabledFor = new List<FeatureFilterConfiguration>();
            if (feature.AlwaysOn ?? false)
            {
                enabledFor.Add(new FeatureFilterConfiguration
                {
                    Name = "AlwaysOn"
                });
            }
            else if ((feature.AlwaysOff ?? false) || !(feature.IsEnabled ?? false))
            {
                enabledFor.Add(new FeatureFilterConfiguration
                {
                    Name = "AlwaysOff"
                });
            }
            else
            {
                if (feature.BrowserRestrictions.Any(br => br.IsActive ?? false))
                {
                    AddBrowserRestrictionFilterSettings(feature, enabledFor);
                }

                if (feature.TimeWindow?.IsActive == true)
                {
                    AddTimeWindowFilterSettings(feature, enabledFor);
                }

                if (feature.RolloutPercentage?.IsActive == true)
                {
                    AddPercentageFilterSettings(feature, enabledFor);
                }

                //Aka Targeting
                if (feature.Audience?.IsActive == true)
                {
                    AddAudienceFilterSetting(feature, enabledFor);
                }
            }

            return new FeatureDefinition
            {
                Name = feature.Name,
                EnabledFor = enabledFor
            };
        }

        private static void AddTimeWindowFilterSettings(Feature feature,
            ICollection<FeatureFilterConfiguration> enabledFor)
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

        private static void AddPercentageFilterSettings(Feature feature,
            ICollection<FeatureFilterConfiguration> enabledFor)
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

        private static void AddBrowserRestrictionFilterSettings(Feature feature,
            ICollection<FeatureFilterConfiguration> enabledFor)
        {
            enabledFor.Add(new FeatureFilterConfiguration
            {
                Name = "Browser",
                Parameters = new BrowserFilterSettings
                {
                    AllowedBrowsers = feature.BrowserRestrictions
                        .Where(b => b.IsActive ?? false)
                        .Select(b =>
                            b.SupportedBrowserId != null
                                ? ((SupportedBrowserId) b.SupportedBrowserId).ToString()
                                : null)
                        .ToList()
                }
            });
        }

        private static void AddAudienceFilterSetting(Feature feature,
            ICollection<FeatureFilterConfiguration> enabledFor)
        {
            enabledFor.Add(new FeatureFilterConfiguration
            {
                Name = "Targeting",
                Parameters = new TargetingFilterSettings
                {
                    Audience = new Microsoft.FeatureManagement.FeatureFilters.Audience
                    {
                        Users = feature.Audience.Users?.Select(u => u.UserName).ToList(),
                        Groups = feature.Audience.GroupRollouts?
                            .Select(g => new Microsoft.FeatureManagement.FeatureFilters.GroupRollout
                            {
                                Name = g.Name,
                                RolloutPercentage = g.RolloutPercentage
                            }).ToList(),
                        DefaultRolloutPercentage = feature.Audience.DefaultRolloutPercentage
                    }
                }
            });
        }

        private IFeatureManagementAppTierAPI CreateFeatureManagementApi()
        {
            //Todo: Wire up service to get bearer tokens
            var credentials = new TokenCredentials("Bearer token here");
            var baseUri = new Uri(_apiOptions.FeatureManagementUri, UriKind.Absolute);
            return new FeatureManagementAppTierAPI(baseUri, credentials);
        }
    }
}