using System;

namespace Microsoft.FeatureManagement.Data.Models
{
    public class TimeWindow : ChangeHistory
    {
        public Guid Id { get; set; }
        public Guid FeatureId { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
    }
}