using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave
{
    public interface IOOBehaveObject
    {
    }
    public interface IOOBehaveObject<T> where T:IOOBehaveObject<T>
    {

    }
}
