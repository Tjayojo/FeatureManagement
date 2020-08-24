using System;

namespace Microsoft.FeatureManagement.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityNameAttribute : Attribute
    {
        public EntityNameAttribute(string entity)
        {
            EntityName = entity ?? throw new ArgumentNullException(nameof(entity));
        }

        public string EntityName;
    }
}