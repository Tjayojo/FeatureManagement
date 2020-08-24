using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Data.Repositories.Interfaces;

namespace Microsoft.FeatureManagement.Data.Repositories.Implementations
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly IMapper _iMapper;
        private readonly FeatureManagementDbContext _dbContext;

        /// <inheritdoc />
        public UserRepository(IMapper iMapper, FeatureManagementDbContext dbContext) : base(iMapper, dbContext)
        {
            _iMapper = iMapper ?? throw new ArgumentNullException(nameof(iMapper));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <inheritdoc />
        public async Task<User> GetByUserName(string userName, CancellationToken cancellationToken = default)
        {
            Models.User user = await _dbContext.Users
                .SingleOrDefaultAsync(u => u.UserName == userName, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            return user == null ? null : _iMapper.Map<Models.User, User>(user);
        }
    }
}