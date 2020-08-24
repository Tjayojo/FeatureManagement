using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Data.Repositories.Interfaces;
using Microsoft.FeatureManagement.Service.Interfaces;

namespace Microsoft.FeatureManagement.Service.Implementations
{
    public class FeatureService : BaseService<Feature>, IFeatureService
    {
        private readonly IFeatureRepository _featureRepository;

        /// <inheritdoc />
        public FeatureService(IFeatureRepository repository) : base(repository)
        {
            _featureRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <inheritdoc />
        public async Task<Feature> GetByName(string featureName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(featureName))
            {
                throw new ArgumentException("Invalid feature name");
            }

            return await _featureRepository
                .GetByName(featureName, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}