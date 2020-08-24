using System.Threading;
using System.Threading.Tasks;
using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Core.Interfaces;

namespace Microsoft.FeatureManagement.Service.Interfaces
{
    public interface IFeatureService : IBaseService<Feature>, IInjectable
    {
        Task<Feature> GetByName(string featureName, CancellationToken cancellationToken = default);
    }
}