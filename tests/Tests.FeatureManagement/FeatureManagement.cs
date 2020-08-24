// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.FeatureManagement.AspNetCore;
using Microsoft.FeatureManagement.Managers;
using Microsoft.FeatureManagement.Providers;
using Microsoft.FeatureManagement.Targeting;
using Xunit;

namespace Tests.FeatureManagement
{
    public class FeatureManagement
    {
        private const string OnFeature = "OnTestFeature";
        private const string OffFeature = "OffFeature";
        private const string ConditionalFeature = "ConditionalFeature";
        private const string ContextualFeature = "ContextualFeature";

        [Fact]
        public async Task ReadsConfiguration()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var services = new ServiceCollection();

            services
                .AddSingleton(config)
                .AddAppTierFeatureManagement(config)
                .AddFeatureFilter<TestFilter>();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            var featureManager = serviceProvider.GetRequiredService<IFeatureManager>();

            Assert.True(await featureManager.IsEnabledAsync(OnFeature));

            Assert.False(await featureManager.IsEnabledAsync(OffFeature));

            var featureFilters = serviceProvider.GetRequiredService<IEnumerable<IFeatureFilterMetadata>>();

            //
            // Sync filter
            var testFeatureFilter = (TestFilter)featureFilters.First(f => f is TestFilter);

            bool called = false;

            testFeatureFilter.Callback = evaluationContext =>
            {
                called = true;

                //Todo
                // Assert.Equal("V1", evaluationContext.Parameters["P1"]);

                Assert.Equal(ConditionalFeature, evaluationContext.FeatureName);

                return true;
            };

            await featureManager.IsEnabledAsync(ConditionalFeature);

            Assert.True(called);
        }

        [Fact]
        public async Task Integrates()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var testServer = new TestServer(WebHost.CreateDefaultBuilder().ConfigureServices(services =>
                {
                    services
                        .AddSingleton(config)
                        .AddAppTierFeatureManagement(config)
                        .AddFeatureFilter<TestFilter>();

                    services.AddMvcCore(o =>
                    {
                        o.EnableEndpointRouting = false;
                        o.Filters.AddForFeature<MvcFilter>(ConditionalFeature);
                    });
                })
            .Configure(app =>
            {

                app.UseForFeature(ConditionalFeature, a => a.Use(async (ctx, next) =>
                {
                    ctx.Response.Headers[nameof(RouterMiddleware)] = bool.TrueString;

                    await next();
                }));

                app.UseMvc();
            }));

            var featureFilters = testServer.Host.Services.GetRequiredService<IEnumerable<IFeatureFilterMetadata>>();

            var testFeatureFilter = (TestFilter)featureFilters.First(f => f is TestFilter);

            testFeatureFilter.Callback = _ => true;

            HttpResponseMessage res = await testServer.CreateClient().GetAsync("");

            Assert.True(res.Headers.Contains(nameof(MvcFilter)));
            Assert.True(res.Headers.Contains(nameof(RouterMiddleware)));

            testFeatureFilter.Callback = _ => false;

            res = await testServer.CreateClient().GetAsync("");

            Assert.False(res.Headers.Contains(nameof(MvcFilter)));
            Assert.False(res.Headers.Contains(nameof(RouterMiddleware)));
        }

        [Fact]
        public async Task GatesFeatures()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var testServer = new TestServer(WebHost.CreateDefaultBuilder().ConfigureServices(services =>
                {
                    services
                        .AddSingleton(config)
                        .AddAppTierFeatureManagement(config)
                        .AddFeatureFilter<TestFilter>();

                    services.AddMvcCore(o => o.EnableEndpointRouting = false);
                })
            .Configure(app => app.UseMvc()));

            var featureFilters = testServer.Host.Services.GetRequiredService<IEnumerable<IFeatureFilterMetadata>>();

            var testFeatureFilter = (TestFilter)featureFilters.First(f => f is TestFilter);

            //
            // Enable all features
            testFeatureFilter.Callback = ctx => true;

            HttpResponseMessage gateAllResponse = await testServer.CreateClient().GetAsync("gateAll");
            HttpResponseMessage gateAnyResponse = await testServer.CreateClient().GetAsync("gateAny");

            Assert.Equal(HttpStatusCode.OK, gateAllResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, gateAnyResponse.StatusCode);

            //
            // Enable 1/2 features
            testFeatureFilter.Callback = ctx => ctx.FeatureName == Enum.GetName(typeof(Features), Features.ConditionalFeature);

            gateAllResponse = await testServer.CreateClient().GetAsync("gateAll");
            gateAnyResponse = await testServer.CreateClient().GetAsync("gateAny");

            Assert.Equal(HttpStatusCode.NotFound, gateAllResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, gateAnyResponse.StatusCode);

            //
            // Enable no
            testFeatureFilter.Callback = ctx => false;

            gateAllResponse = await testServer.CreateClient().GetAsync("gateAll");
            gateAnyResponse = await testServer.CreateClient().GetAsync("gateAny");

            Assert.Equal(HttpStatusCode.NotFound, gateAllResponse.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, gateAnyResponse.StatusCode);
        }

        [Fact]
        public async Task TimeWindow()
        {
            string feature1 = "feature1";
            string feature2 = "feature2";
            string feature3 = "feature3";
            string feature4 = "feature4";

            Environment.SetEnvironmentVariable($"FeatureManagement:{feature1}:EnabledFor:0:Name", "TimeWindow");
            Environment.SetEnvironmentVariable($"FeatureManagement:{feature1}:EnabledFor:0:Parameters:End", DateTimeOffset.UtcNow.AddDays(1).ToString("r"));

            Environment.SetEnvironmentVariable($"FeatureManagement:{feature2}:EnabledFor:0:Name", "TimeWindow");
            Environment.SetEnvironmentVariable($"FeatureManagement:{feature2}:EnabledFor:0:Parameters:End", DateTimeOffset.UtcNow.ToString("r"));

            Environment.SetEnvironmentVariable($"FeatureManagement:{feature3}:EnabledFor:0:Name", "TimeWindow");
            Environment.SetEnvironmentVariable($"FeatureManagement:{feature3}:EnabledFor:0:Parameters:Start", DateTimeOffset.UtcNow.ToString("r"));

            Environment.SetEnvironmentVariable($"FeatureManagement:{feature4}:EnabledFor:0:Name", "TimeWindow");
            Environment.SetEnvironmentVariable($"FeatureManagement:{feature4}:EnabledFor:0:Parameters:Start", DateTimeOffset.UtcNow.AddDays(1).ToString("r"));

            IConfiguration config = new ConfigurationBuilder().AddEnvironmentVariables().Build();

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(config)
                .AddAppTierFeatureManagement(config)
                .AddFeatureFilter<TimeWindowFilter>();

            ServiceProvider provider = serviceCollection.BuildServiceProvider();

            var featureManager = provider.GetRequiredService<IFeatureManager>();

            Assert.True(await featureManager.IsEnabledAsync(feature1));
            Assert.False(await featureManager.IsEnabledAsync(feature2));
            Assert.True(await featureManager.IsEnabledAsync(feature3));
            Assert.False(await featureManager.IsEnabledAsync(feature4));
        }

        [Fact]
        public async Task Percentage()
        {
            string feature1 = "feature1";

            Environment.SetEnvironmentVariable($"FeatureManagement:{feature1}:EnabledFor:0:Name", "Percentage");
            Environment.SetEnvironmentVariable($"FeatureManagement:{feature1}:EnabledFor:0:Parameters:Value", "50");

            IConfiguration config = new ConfigurationBuilder().AddEnvironmentVariables().Build();

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(config)
                .AddAppTierFeatureManagement(config)
                .AddFeatureFilter<PercentageFilter>();

            ServiceProvider provider = serviceCollection.BuildServiceProvider();

            var featureManager = provider.GetRequiredService<IFeatureManager>();

            int enabledCount = 0;

            for (int i = 0; i < 10; i++)
            {
                if (await featureManager.IsEnabledAsync(feature1))
                {
                    enabledCount++;
                }
            }

            Assert.True(enabledCount > 0 && enabledCount < 10);
        }

        [Fact]
        public async Task Targeting()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var services = new ServiceCollection();

            services
                .Configure<FeatureManagementOptions>(options =>
                {
                    options.IgnoreMissingFeatureFilters = true;
                });

            services
                .AddSingleton(config)
                .AddAppTierFeatureManagement(config)
                .AddFeatureFilter<ContextualTargetingFilter>();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            var featureManager = serviceProvider.GetRequiredService<IFeatureManager>();

            string targetingTestFeature = Enum.GetName(typeof(Features), Features.TargetingTestFeature);

            //
            // Targeted by user id
            Assert.True(await featureManager.IsEnabledAsync(targetingTestFeature, new TargetingContext
            {
                UserId = "Jeff"
            }));

            //
            // Not targeted by user id, but targeted by default rollout
            Assert.True(await featureManager.IsEnabledAsync(targetingTestFeature, new TargetingContext
            {
                UserId = "Anne"
            }));

            //
            // Not targeted by user id or default rollout
            Assert.False(await featureManager.IsEnabledAsync(targetingTestFeature, new TargetingContext
            {
                UserId = "Patty"
            }));

            //
            // Targeted by group rollout
            Assert.True(await featureManager.IsEnabledAsync(targetingTestFeature, new TargetingContext
            {
                UserId = "Patty",
                Groups = new List<string> { "Ring1" }
            }));

            //
            // Not targeted by user id, default rollout or group rollout
            Assert.False(await featureManager.IsEnabledAsync(targetingTestFeature, new TargetingContext
            {
                UserId = "Isaac",
                Groups = new List<string> { "Ring1" }
            }));
        }

        [Fact]
        public async Task TargetingAccessor()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var services = new ServiceCollection();

            services
                .Configure<FeatureManagementOptions>(options =>
                {
                    options.IgnoreMissingFeatureFilters = true;
                });

            var targetingContextAccessor = new OnDemandTargetingContextAccessor();

            services.AddSingleton<ITargetingContextAccessor>(targetingContextAccessor);

            services
                .AddSingleton(config)
                .AddAppTierFeatureManagement(config)
                .AddFeatureFilter<TargetingFilter>();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            var featureManager = serviceProvider.GetRequiredService<IFeatureManager>();

            string beta = Enum.GetName(typeof(Features), Features.TargetingTestFeature);

            //
            // Targeted by user id
            targetingContextAccessor.Current = new TargetingContext
            {
                UserId = "Jeff"
            };

            Assert.True(await featureManager.IsEnabledAsync(beta));

            //
            // Not targeted by user id or default rollout
            targetingContextAccessor.Current = new TargetingContext
            {
                UserId = "Patty"
            };

            Assert.False(await featureManager.IsEnabledAsync(beta));
        }

        [Fact]
        public async Task UsesContext()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(config)
                .AddAppTierFeatureManagement(config)
                .AddFeatureFilter<ContextualTestFilter>();

            ServiceProvider provider = serviceCollection.BuildServiceProvider();

            var contextualTestFeatureFilter = (ContextualTestFilter)provider.GetRequiredService<IEnumerable<IFeatureFilterMetadata>>().First(f => f is ContextualTestFilter);

            contextualTestFeatureFilter.ContextualCallback = (ctx, accountContext) =>
            {
                var allowedAccounts = new List<string>();

                //Todo
                // ctx.Parameters.Bind("AllowedAccounts", allowedAccounts);

                return allowedAccounts.Contains(accountContext.AccountId);
            };

            var featureManager = provider.GetRequiredService<IFeatureManager>();

            var context = new AppContext();

            context.AccountId = "NotEnabledAccount";

            Assert.False(await featureManager.IsEnabledAsync(ContextualFeature, context));

            context.AccountId = "abc";

            Assert.True(await featureManager.IsEnabledAsync(ContextualFeature, context));
        }

        [Fact]
        public void LimitsFeatureFilterImplementations()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            Assert.Throws<ArgumentException>(() =>
            {
                new ServiceCollection().AddSingleton(config)
                    .AddAppTierFeatureManagement(config)
                    .AddFeatureFilter<InvalidFeatureFilter>();
            });

            Assert.Throws<ArgumentException>(() =>
            {
                new ServiceCollection().AddSingleton(config)
                    .AddAppTierFeatureManagement(config)
                    .AddFeatureFilter<InvalidFeatureFilter2>();
            });
        }

        [Fact]
        public async Task ListsFeatures()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(config)
                .AddAppTierFeatureManagement(config)
                .AddFeatureFilter<ContextualTestFilter>();

            using (ServiceProvider provider = serviceCollection.BuildServiceProvider())
            {
                var featureManager = provider.GetRequiredService<IFeatureManager>();

                bool hasItems = false;

                await foreach (string feature in featureManager.GetFeatureNamesAsync())
                {
                    hasItems = true;

                    break;
                }

                Assert.True(hasItems);
            }
        }

        [Fact]
        public async Task ThrowsExceptionForMissingFeatureFilter()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var services = new ServiceCollection();

            services
                .AddSingleton(config)
                .AddAppTierFeatureManagement(config);

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            var featureManager = serviceProvider.GetRequiredService<IFeatureManager>();

            var e = await Assert.ThrowsAsync<FeatureManagementException>(async () => await featureManager.IsEnabledAsync(ConditionalFeature));

            Assert.Equal(FeatureManagementError.MissingFeatureFilter, e.Error);
        }

        [Fact]
        public async Task SwallowsExceptionForMissingFeatureFilter()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var services = new ServiceCollection();

            services
                .Configure<FeatureManagementOptions>(options =>
                {
                    options.IgnoreMissingFeatureFilters = true;
                });

            services
                .AddSingleton(config)
                .AddAppTierFeatureManagement(config);

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            var featureManager = serviceProvider.GetRequiredService<IFeatureManager>();

            var isEnabled = await featureManager.IsEnabledAsync(ConditionalFeature);

            Assert.False(isEnabled);
        }

        [Fact]
        public async Task CustomFeatureDefinitionProvider()
        {
            var testFeature = new FeatureDefinition
            {
                Name = ConditionalFeature,
                EnabledFor = new List<FeatureFilterConfiguration>
                {
                    new FeatureFilterConfiguration
                    {
                        Name = "Test"
                        //Todo
                        /*Parameters = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
                        {
                           { "P1", "V1" },
                        }).Build()*/
                    }
                }
            };

            var services = new ServiceCollection();
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            services.AddSingleton<IFeatureDefinitionProvider>(new InMemoryFeatureDefinitionProvider(new[] { testFeature }))
                    .AddAppTierFeatureManagement(config)
                    .AddFeatureFilter<TestFilter>();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            var featureManager = serviceProvider.GetRequiredService<IFeatureManager>();

            var featureFilters = serviceProvider.GetRequiredService<IEnumerable<IFeatureFilterMetadata>>();

            //
            // Sync filter
            var testFeatureFilter = (TestFilter)featureFilters.First(f => f is TestFilter);

            bool called = false;

            testFeatureFilter.Callback = evaluationContext =>
            {
                called = true;

                //Todo
                // Assert.Equal("V1", evaluationContext.Parameters["P1"]);

                Assert.Equal(ConditionalFeature, evaluationContext.FeatureName);

                return true;
            };

            await featureManager.IsEnabledAsync(ConditionalFeature);

            Assert.True(called);
        }
    }
}
