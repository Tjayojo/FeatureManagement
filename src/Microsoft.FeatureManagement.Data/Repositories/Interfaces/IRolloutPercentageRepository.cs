using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Core.Interfaces;

namespace Microsoft.FeatureManagement.Data.Repositories.Interfaces
{
    public interface IRolloutPercentageRepository : IBaseRepository<RolloutPercentage>, IInjectable
    {
    }
}