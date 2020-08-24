using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.FeatureManagement.Core.Attributes;

namespace Microsoft.FeatureManagement.Data.Models
{
    [EntityName("User")]
    public class User : ChangeHistory
    {
        public User()
        {
            Groups = new HashSet<GroupRollout>();
        }

        public Guid Id { get; set; }
        [Required] public string UserName { get; set; }
        public virtual ICollection<GroupRollout> Groups { get; set; }
    }
}