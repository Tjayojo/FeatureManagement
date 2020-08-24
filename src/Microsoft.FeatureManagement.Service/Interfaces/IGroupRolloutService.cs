using System.Threading;
using System.Threading.Tasks;
using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Core.Interfaces;

namespace Microsoft.FeatureManagement.Service.Interfaces
{
    public interface IGroupRolloutService : IBaseService<GroupRollout>, IInjectable
    {
        Task<GroupRollout> GetByName(string name, CancellationToken cancellationToken = default);
    }
}