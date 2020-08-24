using System.Threading;
using System.Threading.Tasks;
using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Core.Interfaces;

namespace Microsoft.FeatureManagement.Service.Interfaces
{
    public interface IUserService : IBaseService<User>, IInjectable
    {
        Task<User> GetByUserName(string userName, CancellationToken cancellationToken = default);
    }
}