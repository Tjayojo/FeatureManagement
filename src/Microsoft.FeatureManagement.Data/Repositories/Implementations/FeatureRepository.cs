using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Data.Repositories.Interfaces;

namespace Microsoft.FeatureManagement.Data.Repositories.Implementations
{
    public class FeatureRepository : BaseRepository<Feature>, IFeatureRepository
    {
        private readonly IMapper _iMapper;
        private readonly FeatureManagementDbContext _dbContext;

        /// <inheritdoc />
        public FeatureRepository(IMapper iMapper, FeatureManagementDbContext dbContext) : base(iMapper, dbContext)
        {
            _iMapper = iMapper ?? throw new ArgumentNullException(nameof(iMapper));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <inheritdoc />
        public async Task<Feature> GetByName(string featureName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(featureName))
            {
                throw new ArgumentException("Invalid feature name");
            }

            Models.Feature feature = await _dbContext.Features
                .SingleOrDefaultAsync(f => f.Name == featureName, cancellationToken)
                .ConfigureAwait(false);
            return _iMapper.Map<Models.Feature, Feature>(feature);
        }
    }
}