using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.FeatureManagement.Core.Attributes;

namespace Microsoft.FeatureManagement.Data.Models
{
    /// <summary>
    /// Defines a percentage of a group to be included in a rollout.
    /// </summary>
    [EntityName("GroupRollout")]
    public class GroupRollout : ChangeHistory
    {
        /// <summary>
        /// Id of the group
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the group.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The percentage of the group that should be considered part of the rollout. Valid values range from 0 to 100 inclusive.
        /// </summary>
        public double RolloutPercentage { get; set; }
    }
}