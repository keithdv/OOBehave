using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave
{


    public interface IValidateMetaProperties
    {
        bool IsValid { get; }
        bool IsSelfValid { get; }
        bool IsChild { get; }
        bool IsBusy { get; }
        bool IsSelfBusy { get; }
    }

    public interface IEditMetaProperties : IValidateMetaProperties
    {
        bool IsModified { get; }
        bool IsSelfModified { get; }
        bool IsNew { get; }
        bool IsSavable { get; }

    }
}
