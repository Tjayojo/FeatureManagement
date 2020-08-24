using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Data.Repositories.Interfaces;

namespace Microsoft.FeatureManagement.Data.Repositories.Implementations
{
    public class ImeWindowRepository : BaseRepository<TimeWindow>, ITImeWindowRepository
    {
        private readonly IMapper _iMapper;
        private readonly FeatureManagementDbContext _dbContext;

        /// <inheritdoc />
        public ImeWindowRepository(IMapper iMapper, FeatureManagementDbContext dbContext) : base(iMapper, dbContext)
        {
            _iMapper = iMapper ?? throw new ArgumentNullException(nameof(iMapper));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <inheritdoc />
        public async Task<TimeWindow> GetByFeatureId(Guid featureId, CancellationToken cancellationToken = default)
        {
            if (featureId == Guid.Empty)
            {
                throw new ArgumentException("Invalid feature id");
            }

            Models.TimeWindow timeWindow = await _dbContext.TimeWindows
                .SingleOrDefaultAsync(tw => tw.FeatureId == featureId, cancellationToken)
                .ConfigureAwait(false);

            return timeWindow == null ? null : _iMapper.Map<Models.TimeWindow, TimeWindow>(timeWindow);
        }
    }
}