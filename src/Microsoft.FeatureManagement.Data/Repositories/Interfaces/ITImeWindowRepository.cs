using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Core.Interfaces;

namespace Microsoft.FeatureManagement.Data.Repositories.Interfaces
{
    public interface ITImeWindowRepository : IBaseRepository<TimeWindow>, IInjectable
    {
        Task<TimeWindow> GetByFeatureId(Guid featureId, CancellationToken cancellationToken = default);
    }
}