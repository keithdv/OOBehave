using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.Portal
{
    public enum PortalOperation
    {
        Create, CreateChild,
        Fetch, FetchChild,
        Insert, InsertChild,
        Update, UpdateChild,
        Delete, DeleteChild,
    }

    public static class PortalOperationExtension
    {
        public static AuthorizationRules.AuthorizeOperation ToAuthorizationOperation(this PortalOperation operation)
        {
            switch (operation)
            {
                case PortalOperation.Create:
                case PortalOperation.CreateChild:
                    return AuthorizationRules.AuthorizeOperation.Create;
                case PortalOperation.Fetch:
                case PortalOperation.FetchChild:
                    return AuthorizationRules.AuthorizeOperation.Fetch;
                case PortalOperation.Insert:
                case PortalOperation.InsertChild:
                case PortalOperation.Update:
                case PortalOperation.UpdateChild:
                    return AuthorizationRules.AuthorizeOperation.Update;
                case PortalOperation.Delete:
                case PortalOperation.DeleteChild:
                    return AuthorizationRules.AuthorizeOperation.Delete;
                default:
                    break;
            }

            throw new Exception($"{operation.ToString()} cannot be converted to AuthorizationOperation");

        }
    }

}
