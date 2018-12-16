﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.Rules
{

    public interface IRule
    {
        uint UniqueIndex { get; }

    }

    public interface IRule<T> : IRule
    {
        Task<RuleResult> Execute(T target);
    }

    public abstract class Rule<T> : IRule<T>
    {

        private static uint indexer = 0;

        protected Rule()
        {
            UniqueIndex = indexer;
            indexer++;
        }


        public uint UniqueIndex { get; }

        // TODO - Pass Cancellation Token and Cancel if we reach this 
        // rule again and it is currently running

        public abstract Task<RuleResult> Execute(T target);

    }
}
