using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.FeatureManagement.Core.Attributes;

namespace Microsoft.FeatureManagement.Data.Models
{
    /// <summary>
    /// An audience definition describing a set of users.
    /// </summary>
    [EntityName("Audience")]
    public class Audience : ChangeHistory
    {
        public Audience()
        {
            Users = new HashSet<User>();
            GroupRollouts = new HashSet<GroupRollout>();
        }
        /// <summary>
        /// Id of the Audience
        /// </summary>
        public Guid Id { get; set; }

        [Required]
        public Guid FeatureId { get; set; }
        public bool IsActive { get; set; }

        /// <summary>
        /// Includes users in the audience by name.
        /// </summary>
        public virtual ICollection<User> Users { get; set; }

        /// <summary>
        /// Includes users in the audience based off a group rollout.
        /// </summary>
        public virtual ICollection<GroupRollout> GroupRollouts { get; set; }

        /// <summary>
        /// Includes users in the audience based off a percentage of the total possible audience. Valid values range from 0 to 100 inclusive.
        /// </summary>
        public double DefaultRolloutPercentage { get; set; }
    }
}