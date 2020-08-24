using System;
using System.Threading.Tasks;
using Microsoft.FeatureManagement.FeatureFilters.Settings;
using Microsoft.FeatureManagement.Targeting;

namespace Microsoft.FeatureManagement.FeatureFilters
{
    /// <summary>
    /// A filter that uses the feature management context to ensure that the current task has the notion of an account id, and that the account id is allowed.
    /// This filter will only be executed if an object implementing <see cref="IAccountContext"/> is passed in during feature evaluation.
    /// </summary>
    [FilterAlias(Alias)]
    public class AccountIdFilter : IContextualFeatureFilter<IAccountContext>
    {
        private const string Alias = "AccountId";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="featureEvaluationContext"></param>
        /// <param name="accountContext"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext featureEvaluationContext,
            IAccountContext accountContext)
        {
            if (string.IsNullOrEmpty(accountContext?.AccountId))
            {
                throw new ArgumentNullException(nameof(accountContext));
            }

            var settings = (AccountFilterSettings) featureEvaluationContext.Parameters;
            return Task.FromResult(settings.AllowedAccounts.Contains(accountContext.AccountId));
        }
    }
}