using System;

namespace Microsoft.FeatureManagement.Data.Models
{
    public class RolloutPercentage : ChangeHistory
    {
        public Guid Id { get; set; }
        public Guid FeatureId { get; set; }
        public bool IsActive { get; set; }
        public int Percentage { get; set; }
    }
}