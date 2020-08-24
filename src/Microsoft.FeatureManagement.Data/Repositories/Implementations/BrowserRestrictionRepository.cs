using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Data.Repositories.Interfaces;

namespace Microsoft.FeatureManagement.Data.Repositories.Implementations
{
    public class BrowserRestrictionRepository : BaseRepository<BrowserRestriction>, IBrowserRestrictionRepository
    {
        private readonly IMapper _iMapper;
        private readonly FeatureManagementDbContext _dbContext;

        /// <inheritdoc />
        public BrowserRestrictionRepository(IMapper iMapper, FeatureManagementDbContext dbContext) : base(iMapper,
            dbContext)
        {
            _iMapper = iMapper ?? throw new ArgumentNullException(nameof(iMapper));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <inheritdoc />
        public async Task<List<BrowserRestriction>> GetByFeatureId(Guid featureId,
            CancellationToken cancellationToken = default)
        {
            if (featureId == Guid.Empty)
            {
                throw new ArgumentException("Invalid feature Id");
            }

            List<Models.BrowserRestriction> browserRestrictions = await _dbContext.BrowserRestrictions
                .Where(br => br.FeatureId == featureId)
                .ToListAsync(cancellationToken).ConfigureAwait(false);

            return browserRestrictions.Count > 0
                ? _iMapper.Map<List<Models.BrowserRestriction>, List<BrowserRestriction>>(browserRestrictions)
                : null;
        }
    }
}