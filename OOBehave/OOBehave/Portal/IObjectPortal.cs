using System.Threading.Tasks;

namespace OOBehave.Portal
{

    // Note: A non generic IObjectPortal with generic functions
    // is a service locator pattern which is bad!!

    public interface IObjectPortal<T> where T : IBase
    {

        Task<T> Create();
        Task<T> Create(object criteria);

        Task<T> CreateChild();
        Task<T> CreateChild(object criteria) ;

        Task Update(T target, object criteria) ;
        Task Update(T target) ;

        Task UpdateChild(T child, object criteria) ;
        Task UpdateChild(T child) ;

        Task Delete(T target, object criteria) ;
        Task Delete(T target) ;

        Task DeleteChild(T child, object criteria) ;
        Task DeleteChild(T child) ;

    }
}