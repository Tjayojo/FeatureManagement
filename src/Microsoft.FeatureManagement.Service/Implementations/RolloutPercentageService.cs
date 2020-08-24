using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Data.Repositories.Interfaces;
using Microsoft.FeatureManagement.Service.Interfaces;

namespace Microsoft.FeatureManagement.Service.Implementations
{
    public class RolloutPercentageService : BaseService<RolloutPercentage>, IRolloutPercentageService
    {
        /// <inheritdoc />
        public RolloutPercentageService(IRolloutPercentageRepository repository) : base(repository)
        {
        }
    }
}