using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Core.Interfaces;

namespace Microsoft.FeatureManagement.Service.Interfaces
{
    public interface ITimeWindowService : IBaseService<TimeWindow>, IInjectable
    {
        Task<TimeWindow> GetByFeatureId(Guid featureId, CancellationToken cancellationToken = default);
    }
}