using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.FeatureManagement.Core.Attributes;

namespace Microsoft.FeatureManagement.Core.DTO
{
    [EntityName("Feature")]
    public class Feature : ChangeHistory
    {
        public Guid Id { get; set; }
        [Required] public string Name { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsArchived { get; set; }
        public bool AlwaysOn { get; set; }
        public bool AlwaysOff { get; set; }
        public TimeWindow TimeWindow { get; set; }
        public RolloutPercentage RolloutPercentage { get; set; }
        public Audience Audience { get; set; }
        public IEnumerable<BrowserRestriction> BrowserRestrictions { get; set; }
    }
}