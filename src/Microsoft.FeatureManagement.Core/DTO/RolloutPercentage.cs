using System;
using Microsoft.FeatureManagement.Core.Attributes;

namespace Microsoft.FeatureManagement.Core.DTO
{
    [EntityName("RolloutPercentage")]
    public class RolloutPercentage : ChangeHistory
    {
        public Guid Id { get; set; }
        public Guid FeatureId { get; set; }
        public bool IsActive { get; set; }
        public int Percentage { get; set; }
    }
}