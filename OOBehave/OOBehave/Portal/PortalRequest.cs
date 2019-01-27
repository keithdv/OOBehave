using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.Portal
{
    public class PortalRequest
    {
        public PortalOperation Operation { get; set; }
        public Type ObjectType { get; set; }
        public Dictionary<Type, byte[]> CriteriaData { get; set; }
        public byte[] ObjectData { get; set; }
    }
}
