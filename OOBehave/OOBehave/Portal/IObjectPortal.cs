using System.Threading.Tasks;

namespace OOBehave.Portal
{

    // Note: A non generic IObjectPortal with generic functions
    // is a service locator pattern which is bad!!

    public interface IReceivePortal<T> where T : IPortalTarget
    {
        Task<T> Create();
        Task<T> Create(object criteria);
        Task<T> Fetch();
        Task<T> Fetch(object criteria);
    }

    public interface IReceivePortalChild<T> where T : IPortalTarget
    {
        Task<T> CreateChild();
        Task<T> CreateChild(object criteria);
        Task<T> FetchChild();
        Task<T> FetchChild(object criteria);
    }

    public interface ISendReceivePortal<T> : IReceivePortal<T> where T : IPortalEditTarget
    {
        Task Update(T target, object criteria);
        Task Update(T target);
        Task Delete(T target, object criteria);
        Task Delete(T target);
    }

    public interface ISendReceivePortalChild<T> : IReceivePortalChild<T> where T : IPortalEditTarget
    {
        Task UpdateChild(T child, object criteria);
        Task UpdateChild(T child);
        Task DeleteChild(T child, object criteria);
        Task DeleteChild(T child);
    }

}