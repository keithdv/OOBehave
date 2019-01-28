using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.Portal
{
    public class PortalResponse
    {
        public Type ObjectType { get; set; }
        public byte[] ObjectData { get; set; }
        public Type ExceptionType { get; set; }
        public byte[] Exception { get; set; }
        public string ExceptionMessage { get; set; }

    }
}
