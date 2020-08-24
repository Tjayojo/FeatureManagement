using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.FeatureManagement.Core.Attributes;

namespace Microsoft.FeatureManagement.Core.DTO
{
    [EntityName("BrowserRestriction")]
    public class BrowserRestriction : ChangeHistory
    {
        public Guid Id { get; set; }
        [Required]
        public Guid FeatureId { get; set; }
        public bool IsActive { get; set; }
        public SupportedBrowserId SupportedBrowserId { get; set; }
    }
}