using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.Portal
{
    public interface IPortalTarget
    {
        Task<IDisposable> StopAllActions();
        void StartAllActions();

        [EditorBrowsable(EditorBrowsableState.Never)]
        void MarkAsChild();

        [EditorBrowsable(EditorBrowsableState.Never)]
        void MarkNew();

        [EditorBrowsable(EditorBrowsableState.Never)]
        void MarkOld();

        [EditorBrowsable(EditorBrowsableState.Never)]
        void MarkClean();

    }

}
