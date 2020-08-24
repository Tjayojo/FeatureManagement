using Microsoft.FeatureManagement.Core.Attributes;

namespace Microsoft.FeatureManagement.Core.DTO
{
    [EntityName("SupportedBrowser")]
    public class SupportedBrowser
    {
        public SupportedBrowserId SupportedBrowserId { get; set; }
        public string Name { get; set; }
    }
}