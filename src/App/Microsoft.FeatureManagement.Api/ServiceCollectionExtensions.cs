using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement.Core;
using Microsoft.FeatureManagement.Data.Repositories.Implementations;
using Microsoft.FeatureManagement.Data.Repositories.Interfaces;
using Microsoft.FeatureManagement.Service.Implementations;
using Microsoft.FeatureManagement.Service.Interfaces;

namespace Microsoft.FeatureManagement.Api
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterAppRepositories(this IServiceCollection iServiceCollection)
        {
            iServiceCollection.AddIInjectableDependencies(typeof(FeatureRepository));
            iServiceCollection.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            iServiceCollection.AddTransient(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        }

        public static void RegisterAppServices(this IServiceCollection iServiceCollection)
        {
            iServiceCollection.AddIInjectableDependencies(typeof(FeatureService));
            iServiceCollection.AddTransient(typeof(IBaseService<>), typeof(BaseService<>));
        }

        public static void RegisterAppAutoMapper(this IServiceCollection iServiceCollection)
        {
            var configuration = new MapperConfiguration(config => config.AddProfile<MappingProfile>());

            IMapper mapper = configuration.CreateMapper();
            iServiceCollection.AddSingleton(mapper);
        }
    }
}