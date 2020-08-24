using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.FeatureManagement.Core.Attributes;

namespace Microsoft.FeatureManagement.Data.Models
{
    [EntityName("Feature")]
    public class Feature : ChangeHistory
    {
        public Feature()
        {
            BrowserRestrictions = new HashSet<BrowserRestriction>();
        }

        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsArchived { get; set; }
        public bool AlwaysOn { get; set; }
        public bool AlwaysOff { get; set; }
        public virtual TimeWindow TimeWindow { get; set; }
        public virtual RolloutPercentage RolloutPercentage { get; set; }
        public virtual Audience Audience { get; set; }
        public virtual ICollection<BrowserRestriction> BrowserRestrictions { get; set; }
    }
}