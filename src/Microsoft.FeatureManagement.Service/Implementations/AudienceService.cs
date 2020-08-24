using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Data.Repositories.Interfaces;
using Microsoft.FeatureManagement.Service.Interfaces;

namespace Microsoft.FeatureManagement.Service.Implementations
{
    public class AudienceService : BaseService<Audience>, IAudienceService
    {
        private readonly IAudienceRepository _repository;

        /// <inheritdoc />
        public AudienceService(IAudienceRepository repository) : base(repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <inheritdoc />
        public async Task<Audience> GetByFeatureId(Guid featureId, CancellationToken cancellationToken = default)
        {
            return await _repository
                .GetByFeatureId(featureId, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}