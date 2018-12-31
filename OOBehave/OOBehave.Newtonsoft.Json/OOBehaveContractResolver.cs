using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OOBehave.Newtonsoft.Json
{
    /// <summary>
    /// The goal of this object is to not have any serialization attributes
    /// on the OOBehave core classes and base classes
    /// </summary>
    public class OOBehaveContractResolver : DefaultContractResolver
    {
        public OOBehaveContractResolver() : base()
        {
        }


    }
}
