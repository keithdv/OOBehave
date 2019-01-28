using OOBehave;
using OOBehave.Portal;
using OOBehave.Rules;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Lib
{
    public class EditObject : EditBase<EditObject>, IEditObject
    {
        public EditObject(IEditBaseServices<EditObject> services, ISendReceivePortalChild<IEditObjectList> editObjectPortal) : base(services)
        {
            RuleManager.AddRule(e =>
            {
                return Value.HasValue ? RuleResult.Empty() : RuleResult.PropertyError(nameof(Value), "Value is required");
            }, nameof(Value));

            // This is not always safe
            // Constructor paramaters do not get disposed on Fat Clients
            // Method Injected portal operations do
            EditObjectPortal = editObjectPortal;
        }

        public Guid? Id { get => Getter<Guid?>(); set => Setter(value); }

        [Required]
        public string Name { get => Getter<string>(); set => Setter(value); }
        public int? Value { get => Getter<int?>(); set => Setter(value); }

        public IEditObjectList Children { get => Getter<IEditObjectList>(); set => Setter(value); }
        public ISendReceivePortalChild<IEditObjectList> EditObjectPortal { get; }

        [Create]
        public async Task Create(string name, int value)
        {
            Name = name;
            Value = value;
            Children = await EditObjectPortal.CreateChild();
            await RuleManager.CheckAllRules();
        }

        [CreateChild]
        public async Task CreateChild()
        {
            await RuleManager.CheckAllRules();
        }

        [CreateChild]
        public void CreateChild(string name, int value)
        {
            Name = name;
            Value = value;
        }

        [Insert]
        [InsertChild]
        public async Task Insert()
        {
            Id = Guid.NewGuid();
            if (Children != null)
            {
                await EditObjectPortal.UpdateChild(Children);
            }
        }

        [Update]
        [UpdateChild]
        public async Task Update()
        {
            Id = Guid.NewGuid();
            if (Children != null)
            {
                await EditObjectPortal.UpdateChild(Children);
            }
        }

        [Delete]
        public void Delete_()
        {
            Id = null;
        }

    }
}
