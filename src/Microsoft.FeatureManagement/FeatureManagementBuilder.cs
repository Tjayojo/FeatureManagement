// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement.FeatureFilters;
using Microsoft.FeatureManagement.Managers;
using Microsoft.FeatureManagement.Targeting;

namespace Microsoft.FeatureManagement
{
    /// <summary>
    /// Provides a way to customize feature management.
    /// </summary>
    public class FeatureManagementBuilder : IFeatureManagementBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public FeatureManagementBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <inheritdoc />
        public IServiceCollection Services { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public IFeatureManagementBuilder AddFeatureFilter<T>() where T : IFeatureFilterMetadata
        {
            Type serviceType = typeof(IFeatureFilterMetadata);

            Type implementationType = typeof(T);

            IEnumerable<Type> featureFilterImplementations = implementationType.GetInterfaces()
                .Where(i => i == typeof(IFeatureFilter) ||
                            i.IsGenericType && i.GetGenericTypeDefinition()
                                .IsAssignableFrom(typeof(IContextualFeatureFilter<>)));

            if (featureFilterImplementations.Count() > 1)
            {
                throw new ArgumentException(
                    "A single feature filter cannot implement more than one feature filter interface.", nameof(T));
            }

            if (!Services.Any(descriptor =>
                descriptor.ServiceType == serviceType && descriptor.ImplementationType == implementationType))
            {
                Services.AddSingleton(typeof(IFeatureFilterMetadata), typeof(T));
            }

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IFeatureManagementBuilder AddSessionManager<T>() where T : ISessionManager
        {
            Services.AddSingleton(typeof(ISessionManager), typeof(T));
            return this;
        }

        /// <inheritdoc />
        public IFeatureManagementBuilder WithBrowserFilter()
        {
            AddFeatureFilter<BrowserFilter>();
            return this;
        }

        /// <inheritdoc />
        public IFeatureManagementBuilder WithTimeWindowFilter()
        {
            AddFeatureFilter<TimeWindowFilter>();
            return this;
        }

        /// <inheritdoc />
        public IFeatureManagementBuilder WithPercentageFilter()
        {
            AddFeatureFilter<PercentageFilter>();
            return this;
        }

        /// <inheritdoc />
        public IFeatureManagementBuilder WithTargetingFilter()
        {
            AddFeatureFilter<TargetingFilter>();
            return this;
        }
    }
}