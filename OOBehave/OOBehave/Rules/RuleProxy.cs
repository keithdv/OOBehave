using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.Rules
{
    public class RuleProxy
    {

        public IValidateBase Target { get; set; }

        internal IPropertyAccess TargetSet => (IPropertyAccess)Target;



    }
}
