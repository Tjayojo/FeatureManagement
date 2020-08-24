using System;
using System.Collections.Generic;
using Microsoft.FeatureManagement.Core.Attributes;

namespace Microsoft.FeatureManagement.Core.DTO
{
    [EntityName("User")]
    public class User : ChangeHistory
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public IEnumerable<GroupRollout> GroupRollouts { get; set; }
    }
}