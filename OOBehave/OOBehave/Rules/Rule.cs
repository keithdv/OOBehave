﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.Rules
{

    public interface IRule
    {
        /// <summary>
        /// Must be unique for every rule across all types
        /// </summary>
        uint UniqueIndex { get; }

    }

    public interface IRule<T> : IRule
    {
        Task<IRuleResult> Execute(T target, CancellationToken token);
    }

    public abstract class Rule<T> : IRule<T>
    {

        private static uint indexer = 0;

        protected Rule()
        {
            /// Must be unique for every rule across all types so Static counter is important
            UniqueIndex = indexer;
            indexer++;
        }


        public uint UniqueIndex { get; }

        // TODO - Pass Cancellation Token and Cancel if we reach this 
        // rule again and it is currently running

        public abstract Task<IRuleResult> Execute(T target, CancellationToken token);

    }
}
