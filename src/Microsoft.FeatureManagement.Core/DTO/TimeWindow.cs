using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.FeatureManagement.Core.Attributes;

namespace Microsoft.FeatureManagement.Core.DTO
{
    [EntityName("TimeWindow")]
    public class TimeWindow : ChangeHistory
    {
        public Guid Id { get; set; }
        [Required]
        public Guid FeatureId { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
    }
}