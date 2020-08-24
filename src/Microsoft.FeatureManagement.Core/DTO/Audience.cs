using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.FeatureManagement.Core.Attributes;

namespace Microsoft.FeatureManagement.Core.DTO
{
    [EntityName("Audience")]
    public class Audience : ChangeHistory
    {
        public Guid Id { get; set; }
        [Required]
        public Guid FeatureId { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<GroupRollout> GroupRollouts { get; set; }
        public double DefaultRolloutPercentage { get; set; }
    }
}