using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Data.Repositories.Interfaces;
using Microsoft.FeatureManagement.Service.Interfaces;

namespace Microsoft.FeatureManagement.Service.Implementations
{
    public class BrowserRestrictionService : BaseService<BrowserRestriction>, IBrowserRestrictionService
    {
        private readonly IBrowserRestrictionRepository _repository;

        /// <inheritdoc />
        public BrowserRestrictionService(IBrowserRestrictionRepository repository) : base(repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <inheritdoc />
        public async Task<List<BrowserRestriction>> GetByFeatureId(Guid featureId,
            CancellationToken cancellationToken = default)
        {
            if (featureId == Guid.Empty)
            {
                throw new ArgumentException("Invalid Feature Id");
            }

            return await _repository.GetByFeatureId(featureId, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}