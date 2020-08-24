using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.FeatureManagement.Data.Extensions
{
    public static class DbExtensions
    {
        public static IServiceCollection AddAppDbContext(this IServiceCollection serviceCollection,
            IConfiguration configuration,
            Action<DbContextOptionsBuilder> options = null)
        {
            return serviceCollection.AddAppDbCOntext<FeatureManagementDbContext>(configuration, options);
        }

        public static IServiceCollection AddAppDbCOntext<TDbContext>(this IServiceCollection serviceCollection,
            IConfiguration configuration, Action<DbContextOptionsBuilder> options = null)
            where TDbContext : DbContext, IFeatureManagementDbContext
        {
            options ??= o => o.UseLazyLoadingProxies().UseSqlServer(configuration["DbConnectionString"]);
            serviceCollection.AddDbContext<TDbContext>(options);

            return serviceCollection;
        }
    }
}