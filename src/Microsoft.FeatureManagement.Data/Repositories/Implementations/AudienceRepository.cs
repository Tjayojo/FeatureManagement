using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Data.Repositories.Interfaces;

namespace Microsoft.FeatureManagement.Data.Repositories.Implementations
{
    public class AudienceRepository : BaseRepository<Audience>, IAudienceRepository
    {
        private readonly IMapper _iMapper;
        private readonly FeatureManagementDbContext _dbContext;

        /// <inheritdoc />
        public AudienceRepository(IMapper iMapper, FeatureManagementDbContext dbContext) : base(iMapper, dbContext)
        {
            _iMapper = iMapper ?? throw new ArgumentNullException(nameof(iMapper));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <inheritdoc />
        public async Task<Audience> GetByFeatureId(Guid featureId, CancellationToken cancellationToken = default)
        {
            if (featureId == Guid.Empty)
            {
                throw new ArgumentException("Invalid Feature Id");
            }

            Models.Audience audience = await _dbContext.Audiences
                .SingleOrDefaultAsync(a => a.FeatureId == featureId, cancellationToken)
                .ConfigureAwait(false);

            return audience == null ? null : _iMapper.Map<Models.Audience, Audience>(audience);
        }
    }
}