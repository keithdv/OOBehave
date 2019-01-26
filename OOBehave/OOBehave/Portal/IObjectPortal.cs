using OOBehave.Portal.Core;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace OOBehave.Portal
{

    // Note: A non generic IObjectPortal with generic functions
    // is a service locator pattern which is bad!!

    public interface IReceivePortal<T> where T : IPortalTarget
    {
        /// <summary>
        /// Use if you want to override the scope used to make dependencies
        /// Advanced: Use at your own risk
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        IServiceScope UsingOperationScope();

        Task<T> Create();
        Task<T> Create<C0>(C0 criteria0);
        Task<T> Create<C0, C1>(C0 criteria0, C1 criteria1);
        Task<T> Create<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2);
        Task<T> Create<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3);
        Task<T> Create<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4);
        Task<T> Create<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5);
        Task<T> Create<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6);
        Task<T> Create<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7);
        Task<T> Fetch();
        Task<T> Fetch<C0>(C0 criteria0);
        Task<T> Fetch<C0, C1>(C0 criteria0, C1 criteria1);
        Task<T> Fetch<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2);
        Task<T> Fetch<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3);
        Task<T> Fetch<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4);
        Task<T> Fetch<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5);
        Task<T> Fetch<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6);
        Task<T> Fetch<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7);

    }

    public interface IReceivePortalChild<T> where T : IPortalTarget
    {
        /// <summary>
        /// Use if you want to override the scope used to make dependencies
        /// Advanced: Use at your own risk
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        IServiceScope UsingOperationScope();

        Task<T> CreateChild();
        Task<T> CreateChild<C0>(C0 criteria0);
        Task<T> CreateChild<C0, C1>(C0 criteria0, C1 criteria1);
        Task<T> CreateChild<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2);
        Task<T> CreateChild<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3);
        Task<T> CreateChild<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4);
        Task<T> CreateChild<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5);
        Task<T> CreateChild<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6);
        Task<T> CreateChild<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7);
        Task<T> FetchChild();
        Task<T> FetchChild<C0>(C0 criteria0);
        Task<T> FetchChild<C0, C1>(C0 criteria0, C1 criteria1);
        Task<T> FetchChild<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2);
        Task<T> FetchChild<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3);
        Task<T> FetchChild<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4);
        Task<T> FetchChild<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5);
        Task<T> FetchChild<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6);
        Task<T> FetchChild<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7);
    }

    public interface ISendReceivePortal<T> : IReceivePortal<T> where T : IPortalEditTarget, IEditMetaProperties
    {
        Task Update(T target);
        Task Update<C0>(T target, C0 criteria0);
        Task Update<C0, C1>(T target, C0 criteria0, C1 criteria1);
        Task Update<C0, C1, C2>(T target, C0 criteria0, C1 criteria1, C2 criteria2);
        Task Update<C0, C1, C2, C3>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3);
        Task Update<C0, C1, C2, C3, C4>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4);
        Task Update<C0, C1, C2, C3, C4, C5>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5);
        Task Update<C0, C1, C2, C3, C4, C5, C6>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6);
        Task Update<C0, C1, C2, C3, C4, C5, C6, C7>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7);
    }

    public interface ISendReceivePortalChild<T> : IReceivePortalChild<T> where T : IPortalEditTarget, IEditMetaProperties
    {
        Task UpdateChild(T target);
        Task UpdateChild<C0>(T target, C0 criteria0);
        Task UpdateChild<C0, C1>(T target, C0 criteria0, C1 criteria1);
        Task UpdateChild<C0, C1, C2>(T target, C0 criteria0, C1 criteria1, C2 criteria2);
        Task UpdateChild<C0, C1, C2, C3>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3);
        Task UpdateChild<C0, C1, C2, C3, C4>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4);
        Task UpdateChild<C0, C1, C2, C3, C4, C5>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5);
        Task UpdateChild<C0, C1, C2, C3, C4, C5, C6>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6);
        Task UpdateChild<C0, C1, C2, C3, C4, C5, C6, C7>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7);

    }

}