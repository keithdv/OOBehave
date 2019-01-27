using OOBehave;
using OOBehave.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public interface IServer
    {
        Task<PortalResponse> Handle(PortalRequest request);
    }

    public class Server<T> : IServer
        where T : class, IEditMetaProperties, IPortalEditTarget
    {
        public Server(IServiceScope scope, IPortalOperationManager<T> portalOperationManager, IZip zip, ISerializer serializer)
        {
            Scope = scope;
            PortalOperationManager = portalOperationManager;
            Zip = zip;
            Serializer = serializer;
        }

        public IServiceScope Scope { get; }
        public IPortalOperationManager<T> PortalOperationManager { get; }
        public IZip Zip { get; }
        public ISerializer Serializer { get; }

        public async Task<PortalResponse> Handle(PortalRequest portalRequest)
        {

            var result = new PortalResponse();

            List<object> criteria = null;
            List<Type> criteriaTypes = null;

            if (portalRequest.CriteriaData != null)
            {
                criteria = new List<object>();
                criteriaTypes = new List<Type>();

                var data = portalRequest.CriteriaData;

                foreach (var kvp in data)
                {
                    var c = Serializer.Deserialize(kvp.Key, Zip.Decompress(kvp.Value));
                    criteria.Add(c);
                    criteriaTypes.Add(kvp.Key);
                }
            }

            T target = null;

            if (portalRequest.ObjectData != null)
            {
                target = Serializer.Deserialize<T>(Zip.Decompress(portalRequest.ObjectData));
            }
            else
            {
                target = Scope.Resolve<T>();
            }

            bool success = false;

            if (criteria == null)
            {
                success = await PortalOperationManager.TryCallOperation(target, portalRequest.Operation);
            }
            else
            {
                success = await PortalOperationManager.TryCallOperation(target, portalRequest.Operation, criteria.ToArray(), criteriaTypes.ToArray());
            }

            if (success)
            {
                result.ObjectData = Zip.Compress(Serializer.Serialize(target));
            }

            return result;
        }

    }
}
