using OOBehave;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib
{
    public interface IEditObjectList : IEditListBase<IEditObject>
    {

    }

    public class EditObjectList : EditListBase<EditObjectList, IEditObject>, IEditObjectList
    {
        public EditObjectList(IEditListBaseServices<EditObjectList, IEditObject> services) : base(services)
        {
        }

    }
}
