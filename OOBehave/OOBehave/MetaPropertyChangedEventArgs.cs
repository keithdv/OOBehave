using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OOBehave
{
    /// <summary>
    /// Meta Property like IsValid, IsModified changed
    /// We need to raise these when a non-meta property has changed - this prevents an infinite loop
    /// </summary>
    public class MetaPropertyChangedEventArgs : PropertyChangedEventArgs
    {
        public MetaPropertyChangedEventArgs(string propertyName) : base(propertyName)
        {
        }
    }
}
