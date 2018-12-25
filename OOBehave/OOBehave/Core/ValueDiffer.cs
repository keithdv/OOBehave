using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.Core
{

    public interface IValuesDiffer
    {
        bool Check<P>(P oldValue, P newValue);
    }

    public class ValuesDiffer : IValuesDiffer
    {
        public bool Check<P>(P oldValue, P newValue)
        {
            if (typeof(P).IsClass)
            {
                if(oldValue == null && newValue == null)
                {
                    return true;
                }
                return !(ReferenceEquals(oldValue, newValue));
            }
            else
            {
                return !oldValue.Equals(newValue);
            }
        }
    }
}
