// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//
using FeatureFlagDemo.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.AspNetCore;
using Microsoft.FeatureManagement.FeatureFilters;
using Microsoft.FeatureManagement.Targeting;

namespace FeatureFlagDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<MvcOptions>(options =>
            {
                options.EnableEndpointRouting = false;
            });

            services.AddAuthentication(Schemes.QueryString)
                    .AddQueryString();

            // Enable the use of IHttpContextAccessor
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddWebTierFeatureManagement(Configuration)
                    .AddFeatureFilter<BrowserFilter>()
                    .AddFeatureFilter<TimeWindowFilter>()
                    .AddFeatureFilter<PercentageFilter>()
                    .AddFeatureFilter<TargetingFilter>()
                    // .WithBrowserFilter()
                    .UseDisabledFeaturesHandler(new FeatureNotEnabledDisabledHandler());

            services.AddMvc(o =>
            {
                // o.Filters.AddForFeature<ThirdPartyActionFilter>(nameof(MyFeatureFlags.EnhancedPipeline));

            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // ReSharper disable once UnusedMember.Global
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // app.UseMiddlewareForFeature<ThirdPartyMiddleware>(nameof(MyFeatureFlags.EnhancedPipeline));

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
