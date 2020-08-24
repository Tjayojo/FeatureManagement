// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//

using System;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.FeatureManagement.Core;
using Microsoft.FeatureManagement.Data.Extensions;
using Microsoft.FeatureManagement.Data.Repositories.Implementations;
using Microsoft.FeatureManagement.Data.Repositories.Interfaces;
using Microsoft.FeatureManagement.Managers;
using Microsoft.FeatureManagement.Providers;
using Microsoft.FeatureManagement.Service.Implementations;
using Microsoft.FeatureManagement.Service.Interfaces;
using Microsoft.FeatureManagement.Targeting;

namespace Microsoft.FeatureManagement
{
    /// <summary>
    /// Extensions used to add feature management functionality.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add required feature management services for an app that is hosted on App tier
        /// </summary>
        /// <param name="services">The service collection that feature management services are added to.</param>
        /// <param name="configuration"></param>
        /// <returns>A <see cref="IFeatureManagementBuilder"/> that can be used to customize feature management functionality.</returns>
        public static IFeatureManagementBuilder AddAppTierFeatureManagement(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.AddFeatureManagement(configuration, SupportedTier.App);
        }

        /// <summary>
        /// Add required feature management services for an web that is hosted on App tier
        /// </summary>
        /// <param name="services">The service collection that feature management services are added to.</param>
        /// <param name="configuration"></param>
        /// <returns>A <see cref="IFeatureManagementBuilder"/> that can be used to customize feature management functionality.</returns>
        public static IFeatureManagementBuilder AddWebTierFeatureManagement(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.AddFeatureManagement(configuration, SupportedTier.Web);
        }

        internal static IFeatureManagementBuilder AddFeatureManagement(this IServiceCollection services,
            IConfiguration configuration, SupportedTier supportedTier)
        {
            #region Parameter Validation

            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            #endregion

            services.AddLogging();
            
            services.AddSingleton<ITargetingContextAccessor, HttpContextTargetingContextAccessor>();

            AddTierBasedServices(services, configuration, supportedTier);

            services.TryAddScoped<IFeatureManager, FeatureManager>();

            services.TryAddScoped<ISessionManager, EmptySessionManager>();

            services.AddScoped<IFeatureManagerSnapshot, FeatureManagerSnapshot>();

            return new FeatureManagementBuilder(services);
        }

        internal static void AddTierBasedServices(IServiceCollection services, IConfiguration configuration,
            SupportedTier supportedTier)
        {
            switch (supportedTier)
            {
                case SupportedTier.App:
                    services.AddAppDbContext(configuration);
                    services.RegisterAppAutoMapper();
                    services.RegisterAppRepositories();
                    services.RegisterAppServices();
                    services.TryAddScoped<IFeatureDefinitionProvider, EfCoreFeatureDefinitionProvider>();
                    break;
                case SupportedTier.Web:
                    services.Configure<ApiOptions>(options =>
                        options.FeatureManagementUri = configuration["FeatureManagement.Api.Endpoint"]);
                    services.TryAddScoped<IFeatureDefinitionProvider, ApiFeatureDefinitionProvider>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(supportedTier), supportedTier, null);
            }
        }

        internal static void RegisterAppRepositories(this IServiceCollection services)
        {
            services.AddIInjectableDependencies(typeof(FeatureRepository));
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddTransient(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        }

        internal static void RegisterAppServices(this IServiceCollection services)
        {
            services.AddIInjectableDependencies(typeof(FeatureService));
            services.AddTransient(typeof(IBaseService<>), typeof(BaseService<>));
        }

        internal static void RegisterAppAutoMapper(this IServiceCollection services)
        {
            var configuration = new MapperConfiguration(config => config.AddProfile<MappingProfile>());

            IMapper mapper = configuration.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}