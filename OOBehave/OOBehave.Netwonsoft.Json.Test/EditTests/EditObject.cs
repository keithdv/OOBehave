using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace OOBehave.Netwonsoft.Json.Test.EditTests
{
    public interface IEditObject : IEditBase
    {
        Guid ID { get; set; }
        string Name { get; set; }

        IEditObject Child { get; set; }
        IEditObject Parent { get; set; }
    }

    public class EditObject : EditBase, IEditObject
    {
        public EditObject(IEditBaseServices<EditObject> services) : base(services)
        {
        }

        public Guid ID { get => Getter<Guid>(); set => Setter(value); }
        public string Name { get => Getter<string>(); set => Setter(value); }
        public IEditObject Child { get => Getter<IEditObject>(); set => Setter(value); }
        public IEditObject Parent { get => Getter<IEditObject>(); set => Setter(value); }

    }
}
