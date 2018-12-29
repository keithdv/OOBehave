using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace OOBehave.Netwonsoft.Json.Test.BaseTests
{
    public interface IBaseObject : IBase
    {
        Guid ID { get; set; }
        string Name { get; set; }

        IBaseObject Child { get; set; }
        IBaseObject Parent { get; set; }
    }

    public class BaseObject : Base, IBaseObject
    {
        public BaseObject(IBaseServices<BaseObject> services) : base(services)
        {
        }

        public Guid ID { get => Getter<Guid>(); set => Setter(value); }
        public string Name { get => Getter<string>(); set => Setter(value); }
        public IBaseObject Child { get => Getter<IBaseObject>(); set => Setter(value); }
        public IBaseObject Parent { get => Getter<IBaseObject>(); set => Setter(value); }

    }
}
