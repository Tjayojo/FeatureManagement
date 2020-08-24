using Microsoft.Rest;

namespace Microsoft.FeatureManagement
{
    public class ApiOptions
    {
        public string FeatureManagementUri { get; set; }
        public ServiceClientCredentials Credentials { get; set; }
    }
}