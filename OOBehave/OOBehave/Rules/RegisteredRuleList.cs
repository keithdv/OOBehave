using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.Rules
{

    public interface IRegisteredRuleList : IReadOnlyCollection<IRule>
    {

    }

    public class RegisteredRuleList : ConcurrentBag<IRule>, IRegisteredRuleList
    {


    }
}
