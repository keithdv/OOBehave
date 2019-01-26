using OOBehave;
using System;

namespace _3Tier.Lib
{
    public class EditObject : EditBase<EditObject>, IEditObject
    {
        public EditObject(IEditBaseServices<EditObject> services) : base(services)
        {
        }

        public Guid Id { get => Getter<Guid>(); set => Setter(value); }
        public string Name { get => Getter<string>(); set => Setter(value); }
        public int? Value { get => Getter<int?>(); set => Setter(value); }

    }
}
