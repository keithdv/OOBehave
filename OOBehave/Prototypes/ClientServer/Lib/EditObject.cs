using OOBehave;
using OOBehave.Portal;
using System;

namespace Lib
{
    public class EditObject : EditBase<EditObject>, IEditObject
    {
        public EditObject(IEditBaseServices<EditObject> services) : base(services)
        {
        }

        public Guid Id { get => Getter<Guid>(); set => Setter(value); }
        public string Name { get => Getter<string>(); set => Setter(value); }
        public int? Value { get => Getter<int?>(); set => Setter(value); }

        public IEditObject Child { get => Getter<IEditObject>(); set => Setter(value); }


        [Create]
        [CreateChild]
        public void Create(string name, int value)
        {
            Name = name;
            Value = value;
        }

        [Insert]
        public void Insert()
        {
            Id = Guid.NewGuid();
        }
    }
}
