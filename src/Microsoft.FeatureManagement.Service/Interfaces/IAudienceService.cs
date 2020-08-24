using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Core.Interfaces;

namespace Microsoft.FeatureManagement.Service.Interfaces
{
    public interface IAudienceService : IBaseService<Audience>, IInjectable
    {
        Task<Audience> GetByFeatureId(Guid featureId, CancellationToken cancellationToken = default);
    }
}