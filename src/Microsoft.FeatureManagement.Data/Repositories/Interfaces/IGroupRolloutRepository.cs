using System.Threading;
using System.Threading.Tasks;
using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Core.Interfaces;

namespace Microsoft.FeatureManagement.Data.Repositories.Interfaces
{
    public interface IGroupRolloutRepository : IBaseRepository<GroupRollout>, IInjectable
    {
        Task<GroupRollout> GetByName(string name, CancellationToken cancellationToken = default);
    }
}