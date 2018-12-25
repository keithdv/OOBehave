using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.Portal
{
    public interface IPortalTarget
    {
        Task<IDisposable> StopAllActions();
        void StartAllActions();
    }
}
