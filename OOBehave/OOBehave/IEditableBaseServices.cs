using OOBehave.Core;
using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave
{
    /// <summary>
    /// Wrap the OOBehaveBase services into an interface so that 
    /// the inheriting classes don't need to list all services
    /// and services can be added
    /// </summary>
    public interface IEditableBaseServices<T> : IValidateBaseServices<T>
    {
    }

    public class EditableBaseServices<T> : ValidateBaseServices<T>, IEditableBaseServices<T>
    {

        public EditableBaseServices(IRegisteredPropertyValidateDataManager<T> registeredPropertyManager, IRegisteredRuleManager ruleManager) : base(registeredPropertyManager, ruleManager) { }
    }
}
