using System.Threading.Tasks;

namespace OOBehave.Portal
{

    // Note: A non generic IObjectPortal with generic functions
    // is a service locator pattern which is bad!!

    public interface IReceivePortal<T> where T : IPortalTarget
    {
        Task<T> Create();
        Task<T> Create(params object[] criteria);
        Task<T> Fetch();
        Task<T> Fetch(params object[] criteria);
    }

    public interface IReceivePortalChild<T> where T : IPortalTarget
    {
        Task<T> CreateChild();
        Task<T> CreateChild(params object[] criteria);
        Task<T> FetchChild();
        Task<T> FetchChild(params object[] criteria);
    }

    public interface ISendReceivePortal<T> : IReceivePortal<T> where T : IPortalEditTarget, IEditMetaProperties
    {
        Task Update(T target, params object[] criteria);
        Task Update(T target);
    }

    public interface ISendReceivePortalChild<T> : IReceivePortalChild<T> where T : IPortalEditTarget, IEditMetaProperties
    {
        Task UpdateChild(T child, params object[] criteria);
        Task UpdateChild(T child);

    }

}