using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Data.Repositories.Interfaces;
using Microsoft.FeatureManagement.Service.Interfaces;

namespace Microsoft.FeatureManagement.Service.Implementations
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IUserRepository _userRepository;

        /// <inheritdoc />
        public UserService(IUserRepository userRepository) : base(userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<User> GetByUserName(string userName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Invalid UserName");
            }

            return await _userRepository
                .GetByUserName(userName, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}