using AutoMapper;
using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Data.Repositories.Interfaces;

namespace Microsoft.FeatureManagement.Data.Repositories.Implementations
{
    public class RolloutPercentageRepository : BaseRepository<RolloutPercentage>, IRolloutPercentageRepository
    {
        /// <inheritdoc />
        public RolloutPercentageRepository(IMapper iMapper, FeatureManagementDbContext dbContext) : base(iMapper, dbContext)
        {
        }
    }
}