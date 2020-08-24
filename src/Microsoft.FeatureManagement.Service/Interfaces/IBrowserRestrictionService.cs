using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Core.Interfaces;

namespace Microsoft.FeatureManagement.Service.Interfaces
{
    public interface IBrowserRestrictionService : IBaseService<BrowserRestriction>, IInjectable
    {
        Task<List<BrowserRestriction>> GetByFeatureId(Guid featureId, CancellationToken cancellationToken = default);
    }
}