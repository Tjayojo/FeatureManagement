using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Data.Repositories.Interfaces;

namespace Microsoft.FeatureManagement.Data.Repositories.Implementations
{
    public class GroupRolloutRepository : BaseRepository<GroupRollout>, IGroupRolloutRepository
    {
        private readonly IMapper _iMapper;
        private readonly FeatureManagementDbContext _dbContext;

        /// <inheritdoc />
        public GroupRolloutRepository(IMapper iMapper, FeatureManagementDbContext dbContext) : base(iMapper, dbContext)
        {
            _iMapper = iMapper ?? throw new ArgumentNullException(nameof(iMapper));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <inheritdoc />
        public async Task<GroupRollout> GetByName(string name, CancellationToken cancellationToken = default)
        {
            Models.GroupRollout groupRollout = await _dbContext.GroupRollouts
                .SingleOrDefaultAsync(u => u.Name == name, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            
            return groupRollout == null ? null : _iMapper.Map<Models.GroupRollout, GroupRollout>(groupRollout);
        }
    }
}