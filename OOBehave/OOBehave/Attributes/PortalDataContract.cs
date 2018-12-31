using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.Attributes
{

    [System.AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class PortalDataContractAttribute : Attribute
    {
        readonly string positionalString;

        // This is a positional argument
        public PortalDataContractAttribute()
        {
        }

    }

    [System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class PortalDataMemberAttribute : Attribute
    {

        // This is a positional argument
        public PortalDataMemberAttribute()
        {
        }

    }

}
