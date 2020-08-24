using System;

namespace Microsoft.FeatureManagement.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class FeatureKeyName : Attribute
    {
        private string _key;

        public FeatureKeyName(string key)
        {
            _key = key ?? throw new ArgumentNullException(nameof(key));
        }

        public string GetFeatureKey() => _key;
    }
}