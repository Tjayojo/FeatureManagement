using System.Threading;
using System.Threading.Tasks;
using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Core.Interfaces;

namespace Microsoft.FeatureManagement.Data.Repositories.Interfaces
{
    public interface IFeatureRepository : IBaseRepository<Feature>, IInjectable
    {
        Task<Feature> GetByName(string featureName, CancellationToken cancellationToken = default);
    }
}