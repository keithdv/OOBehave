﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.Rules
{
    public interface ICascadeRule<T> : IRule<T>
    {

        IEnumerable<string> TriggerProperties { get; }

    }

    public abstract class CascadeAsyncRule<T> : Rule<T>, ICascadeRule<T>
    {

        public CascadeAsyncRule() : base()
        {
            TriggerProperties = new List<string>();
        }

        IEnumerable<string> ICascadeRule<T>.TriggerProperties => TriggerProperties.AsEnumerable();
        protected List<string> TriggerProperties { get; }

    }

    public abstract class CascadeRule<T> : CascadeAsyncRule<T>
    {
        public CascadeRule() : base()
        {

        }

        public abstract IRuleResult Execute(T target);

        public sealed override Task<IRuleResult> Execute(T target, CancellationToken token)
        {
            return Task.FromResult(Execute(target));
        }

    }
}