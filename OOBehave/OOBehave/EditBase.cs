using OOBehave.Core;
using System;

namespace OOBehave
{

    public interface IEditBase : IValidateBase
    {

    }

    public interface IEditBase<T> : IEditBase, IValidateBase<T>
    {

    }

    public abstract class EditBase<T> : ValidateBase<T>, IOOBehaveObject<T>, IEditBase<T>
        where T : EditBase<T>
    {


        public EditBase(IEditableBaseServices<T> services) : base(services)
        {
        }


    }



}
