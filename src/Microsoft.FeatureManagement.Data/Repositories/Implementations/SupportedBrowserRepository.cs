using AutoMapper;
using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Data.Repositories.Interfaces;

namespace Microsoft.FeatureManagement.Data.Repositories.Implementations
{
    public class SupportedBrowserRepository : BaseRepository<SupportedBrowser>, ISupportedBrowserRepository
    {
        public SupportedBrowserRepository(IMapper iMapper, FeatureManagementDbContext dbContext) : base(iMapper,
            dbContext)
        {
        }
    }
}