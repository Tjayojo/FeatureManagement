using System;
using Microsoft.FeatureManagement.Core.Attributes;

namespace Microsoft.FeatureManagement.Data.Models
{
    [EntityName("BrowserRestriction")]
    public class BrowserRestriction : ChangeHistory
    {
        public Guid Id { get; set; }
        public Guid FeatureId { get; set; }
        public bool IsActive { get; set; }
        public SupportedBrowserId SupportedBrowserId { get; set; }
    }
}