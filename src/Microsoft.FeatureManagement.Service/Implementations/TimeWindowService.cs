using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Data.Repositories.Interfaces;
using Microsoft.FeatureManagement.Service.Interfaces;

namespace Microsoft.FeatureManagement.Service.Implementations
{
    public class TimeWindowService : BaseService<TimeWindow>, ITimeWindowService
    {
        private readonly ITImeWindowRepository _repository;

        /// <inheritdoc />
        public TimeWindowService(ITImeWindowRepository repository) : base(repository)
        {
            _repository = repository;
        }

        /// <inheritdoc />
        public async Task<TimeWindow> GetByFeatureId(Guid featureId, CancellationToken cancellationToken = default)
        {
            if (featureId == Guid.Empty)
            {
                throw new ArgumentException("Invalid feature id");
            }

            return await _repository.GetByFeatureId(featureId, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}