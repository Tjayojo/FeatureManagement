using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Data.Repositories.Interfaces;
using Microsoft.FeatureManagement.Service.Interfaces;

namespace Microsoft.FeatureManagement.Service.Implementations
{
    public class GroupRolloutService : BaseService<GroupRollout>, IGroupRolloutService
    {
        private readonly IGroupRolloutRepository _repository;

        /// <inheritdoc />
        public GroupRolloutService(IGroupRolloutRepository repository) : base(repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <inheritdoc />
        public async Task<GroupRollout> GetByName(string name, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Invalid UserName");
            }

            return await _repository
                .GetByName(name, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}