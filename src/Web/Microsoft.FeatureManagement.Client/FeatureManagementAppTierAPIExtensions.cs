// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.FeatureManagement.Client
{
    using Models;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for FeatureManagementAppTierAPI.
    /// </summary>
    public static partial class FeatureManagementAppTierAPIExtensions
    {
            /// <summary>
            /// Get all audiences
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<Audience>> GetAllAudiencesAsync(this IFeatureManagementAppTierAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetAllAudiencesWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Create a new audience
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='body'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> PostAudienceAsync(this IFeatureManagementAppTierAPI operations, Audience body = default(Audience), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.PostAudienceWithHttpMessagesAsync(body, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Delete a audience
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ProblemDetails> DeleteAudienceAsync(this IFeatureManagementAppTierAPI operations, System.Guid? id = default(System.Guid?), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.DeleteAudienceWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Get audience by Id
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetAudienceByIdAsync(this IFeatureManagementAppTierAPI operations, System.Guid id, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetAudienceByIdWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Update a audience
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='body'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> PutAudienceAsync(this IFeatureManagementAppTierAPI operations, System.Guid id, Audience body = default(Audience), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.PutAudienceWithHttpMessagesAsync(id, body, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Get audience by Feature Id
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='featureId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetAudienceByFeatureIdAsync(this IFeatureManagementAppTierAPI operations, System.Guid featureId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetAudienceByFeatureIdWithHttpMessagesAsync(featureId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Get all Browser Restrictions
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<BrowserRestriction>> GetAllBrowserRestrictionsAsync(this IFeatureManagementAppTierAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetAllBrowserRestrictionsWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Create a new Browser Restriction
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='body'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> PostBrowserRestrictionAsync(this IFeatureManagementAppTierAPI operations, BrowserRestriction body = default(BrowserRestriction), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.PostBrowserRestrictionWithHttpMessagesAsync(body, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Delete a Browser Restriction
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ProblemDetails> DeleteBrowserRestrictionAsync(this IFeatureManagementAppTierAPI operations, System.Guid? id = default(System.Guid?), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.DeleteBrowserRestrictionWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Get Browser Restriction by Id
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetBrowserRestrictionByIdAsync(this IFeatureManagementAppTierAPI operations, System.Guid id, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetBrowserRestrictionByIdWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Update a Browser Restriction
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='body'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> PutBrowserRestrictionAsync(this IFeatureManagementAppTierAPI operations, System.Guid id, BrowserRestriction body = default(BrowserRestriction), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.PutBrowserRestrictionWithHttpMessagesAsync(id, body, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Get Browser Restriction by feature Id
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='featureId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetBrowserRestrictionByFeatureIdAsync(this IFeatureManagementAppTierAPI operations, System.Guid featureId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetBrowserRestrictionByFeatureIdWithHttpMessagesAsync(featureId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Get all features
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<Feature>> GetAllFeaturesAsync(this IFeatureManagementAppTierAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetAllFeaturesWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Create a new feature
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='body'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> PostFeatureAsync(this IFeatureManagementAppTierAPI operations, Feature body = default(Feature), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.PostFeatureWithHttpMessagesAsync(body, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Delete a feature
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ProblemDetails> DeleteFeatureAsync(this IFeatureManagementAppTierAPI operations, System.Guid? id = default(System.Guid?), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.DeleteFeatureWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Get a feature by name
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='name'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetFeatureByNameAsync(this IFeatureManagementAppTierAPI operations, string name, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetFeatureByNameWithHttpMessagesAsync(name, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Get feature by Id
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetFeatureByIdAsync(this IFeatureManagementAppTierAPI operations, System.Guid id, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetFeatureByIdWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Update a feature
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='body'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> PutFeatureAsync(this IFeatureManagementAppTierAPI operations, System.Guid id, Feature body = default(Feature), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.PutFeatureWithHttpMessagesAsync(id, body, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Get all Group Rollouts
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<GroupRollout>> GetAllGroupRolloutsAsync(this IFeatureManagementAppTierAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetAllGroupRolloutsWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Create a new Group Rollout
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='body'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> PostGroupRolloutAsync(this IFeatureManagementAppTierAPI operations, GroupRollout body = default(GroupRollout), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.PostGroupRolloutWithHttpMessagesAsync(body, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Delete a Group Rollout
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ProblemDetails> DeleteGroupRolloutAsync(this IFeatureManagementAppTierAPI operations, System.Guid? id = default(System.Guid?), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.DeleteGroupRolloutWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Get a Group Rollout by name
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='groupRolloutName'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetGroupRolloutByNameAsync(this IFeatureManagementAppTierAPI operations, string groupRolloutName, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetGroupRolloutByNameWithHttpMessagesAsync(groupRolloutName, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Get Group Rollout by Id
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetGroupRolloutByIdAsync(this IFeatureManagementAppTierAPI operations, System.Guid id, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetGroupRolloutByIdWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Update a Group Rollout
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='body'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> PutGroupRolloutAsync(this IFeatureManagementAppTierAPI operations, System.Guid id, GroupRollout body = default(GroupRollout), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.PutGroupRolloutWithHttpMessagesAsync(id, body, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Get all Time Windows
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<TimeWindow>> GetAllTimeWindowsAsync(this IFeatureManagementAppTierAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetAllTimeWindowsWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Create a new Time Window
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='body'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> PostTimeWindowAsync(this IFeatureManagementAppTierAPI operations, TimeWindow body = default(TimeWindow), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.PostTimeWindowWithHttpMessagesAsync(body, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Delete a Time Window
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ProblemDetails> DeleteTimeWindowAsync(this IFeatureManagementAppTierAPI operations, System.Guid? id = default(System.Guid?), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.DeleteTimeWindowWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Get Time Window by Id
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetTimeWindowByIdAsync(this IFeatureManagementAppTierAPI operations, System.Guid id, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetTimeWindowByIdWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Update a Time Window
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='body'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> PutTimeWindowAsync(this IFeatureManagementAppTierAPI operations, System.Guid id, TimeWindow body = default(TimeWindow), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.PutTimeWindowWithHttpMessagesAsync(id, body, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Get Time Window by feature Id
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='featureId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetTimeWindowByFeatureIdAsync(this IFeatureManagementAppTierAPI operations, System.Guid featureId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetTimeWindowByFeatureIdWithHttpMessagesAsync(featureId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Get all users
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<User>> GetAllUsersAsync(this IFeatureManagementAppTierAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetAllUsersWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Create a new user
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='body'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> PostUserAsync(this IFeatureManagementAppTierAPI operations, User body = default(User), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.PostUserWithHttpMessagesAsync(body, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Delete a user
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ProblemDetails> DeleteUserAsync(this IFeatureManagementAppTierAPI operations, System.Guid? id = default(System.Guid?), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.DeleteUserWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Get a user by UserName
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='userName'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetUserByUsernameAsync(this IFeatureManagementAppTierAPI operations, string userName, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetUserByUsernameWithHttpMessagesAsync(userName, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Get user by Id
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetUserByIdAsync(this IFeatureManagementAppTierAPI operations, System.Guid id, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetUserByIdWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Update a user
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='body'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> PutUserAsync(this IFeatureManagementAppTierAPI operations, System.Guid id, User body = default(User), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.PutUserWithHttpMessagesAsync(id, body, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}