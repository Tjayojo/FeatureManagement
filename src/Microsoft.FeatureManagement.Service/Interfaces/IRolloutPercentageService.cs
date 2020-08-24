using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Core.Interfaces;

namespace Microsoft.FeatureManagement.Service.Interfaces
{
    public interface IRolloutPercentageService : IBaseService<RolloutPercentage>, IInjectable
    {
    }
}