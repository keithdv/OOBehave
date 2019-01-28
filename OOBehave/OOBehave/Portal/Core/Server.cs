using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.Portal.Core
{
    public interface IServer
    {
        Task<PortalResponse> Handle(PortalRequest request);
    }

    public class Server : IServer
    {
        public Server(IServiceScope scope, GetPortalOperationManager portalOperationManager, IZip zip, ISerializer serializer)
        {
            Scope = scope;
            GetPortalOperationManager = portalOperationManager;
            Zip = zip;
            Serializer = serializer;
        }

        public IServiceScope Scope { get; }
        public GetPortalOperationManager GetPortalOperationManager { get; }
        public IZip Zip { get; }
        public ISerializer Serializer { get; }

        public async Task<PortalResponse> Handle(PortalRequest portalRequest)
        {

            var type = portalRequest.ObjectType;
            var response = new PortalResponse();
            response.ObjectType = type;
            try
            {

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

                IPortalTarget target = null;

                if (portalRequest.ObjectData != null)
                {
                    target = Serializer.Deserialize(type, Zip.Decompress(portalRequest.ObjectData)) as IPortalTarget ?? throw new Exception($"{type.FullName} does not implement IPortalTarget");
                }
                else
                {
                    target = Scope.Resolve(type) as IPortalTarget ?? throw new Exception($"{type.FullName} does not implement IPortalTarget");
                }

                bool success = false;

                var portalOperationManager = GetPortalOperationManager(type);

                // TODO Split out to insert, delete, update for Update operation

                if (criteria == null)
                {
                    success = await portalOperationManager.TryCallOperation(target, portalRequest.Operation);
                }
                else
                {
                    success = await portalOperationManager.TryCallOperation(target, portalRequest.Operation, criteria.ToArray(), criteriaTypes.ToArray());
                }

                if (!success && (criteria != null || portalRequest.Operation != PortalOperation.Create))
                {
                    if (criteria == null)
                    {
                        throw new OperationMethodCallFailedException($"{portalRequest.Operation.ToString()} method with no criteria not found on {target.GetType().FullName}.");
                    }
                    else
                    {
                        throw new OperationMethodCallFailedException($"{portalRequest.Operation.ToString()} method on {target.GetType().FullName} with criteria [{string.Join(", ", criteriaTypes.Select(x => x.FullName))}] not found.");
                    }
                }

                if (target != null)
                {
                    response.ObjectData = Zip.Compress(Serializer.Serialize(target));
                }
            }
            catch (Exception ex)
            {
                response.Exception = Zip.Compress(Serializer.Serialize(ex));
                response.ExceptionType = ex.GetType();
                response.ExceptionMessage = ErrorMessage(ex);
            }

            return response;
        }

        private string ErrorMessage(Exception ex)
        {
            if (ex.InnerException == null)
            {
                return ex.Message;
            }
            else
            {
                return ex.Message + "; " + ErrorMessage(ex.InnerException);
            }
        }
    }
}
