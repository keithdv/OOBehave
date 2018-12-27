using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.Portal
{
    public enum PortalOperation
    {
        Create, CreateChild,
        Fetch, FetchChild,
        Delete, DeleteChild,
        Update, UpdateChild
    }

    public static class PortalOperationExtension
    {
        public static AuthorizationRules.AuthorizeOperation ToAuthorizationOperation(this PortalOperation operation)
        {
            switch (operation)
            {
                case PortalOperation.Create:
                    return AuthorizationRules.AuthorizeOperation.Create;
                case PortalOperation.CreateChild:
                    return AuthorizationRules.AuthorizeOperation.Create;
                case PortalOperation.Fetch:
                    return AuthorizationRules.AuthorizeOperation.Fetch;
                case PortalOperation.FetchChild:
                    return AuthorizationRules.AuthorizeOperation.Fetch;
                case PortalOperation.Delete:
                    return AuthorizationRules.AuthorizeOperation.Delete;
                case PortalOperation.DeleteChild:
                    return AuthorizationRules.AuthorizeOperation.Delete;
                case PortalOperation.Update:
                    return AuthorizationRules.AuthorizeOperation.Update;
                case PortalOperation.UpdateChild:
                    return AuthorizationRules.AuthorizeOperation.Update;
                default:
                    break;
            }

            throw new Exception($"{operation.ToString()} cannot be converted to AuthorizationOperation");

        }
    }
  
}
