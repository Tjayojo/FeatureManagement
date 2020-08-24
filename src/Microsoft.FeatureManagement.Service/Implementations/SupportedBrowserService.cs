using Microsoft.FeatureManagement.Core.DTO;
using Microsoft.FeatureManagement.Data.Repositories.Interfaces;
using Microsoft.FeatureManagement.Service.Interfaces;

namespace Microsoft.FeatureManagement.Service.Implementations
{
    public class SupportedBrowserService : BaseService<SupportedBrowser>, ISupportedBrowserService
    {
        public SupportedBrowserService(ISupportedBrowserRepository repository) : base(repository)
        {
        }
    }
}