using System;

namespace Microsoft.FeatureManagement.Core.DTO
{
    public class ChangeHistory
    {
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }
    }
}