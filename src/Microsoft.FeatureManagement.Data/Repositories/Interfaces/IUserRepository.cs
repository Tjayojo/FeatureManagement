using System.Threading;
using System.Threading.Tasks;
using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Core.Interfaces;

namespace Microsoft.FeatureManagement.Data.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>, IInjectable
    {
        Task<User> GetByUserName(string userName, CancellationToken cancellationToken = default);
    }
}