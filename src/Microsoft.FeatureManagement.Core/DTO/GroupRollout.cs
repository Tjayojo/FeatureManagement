using System;
using Microsoft.FeatureManagement.Core.Attributes;

namespace Microsoft.FeatureManagement.Core.DTO
{
    [EntityName("GroupRollout")]
    public class GroupRollout : ChangeHistory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double RolloutPercentage { get; set; }
    }
}