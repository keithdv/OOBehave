using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.Portal
{
    public enum Operation
    {
        Create, CreateChild,
        Fetch, FetchChild,
        Delete, DeleteChild,
        Update, UpdateChild
    }

    public static class OperationExtension
    {
        public static AuthorizationRules.AuthorizeOperation ToAuthorizationOperation(this Operation operation)
        {
            switch (operation)
            {
                case Operation.Create:
                    return AuthorizationRules.AuthorizeOperation.Create;
                case Operation.CreateChild:
                    return AuthorizationRules.AuthorizeOperation.Create;
                case Operation.Fetch:
                    return AuthorizationRules.AuthorizeOperation.Fetch;
                case Operation.FetchChild:
                    return AuthorizationRules.AuthorizeOperation.Fetch;
                case Operation.Delete:
                    return AuthorizationRules.AuthorizeOperation.Delete;
                case Operation.DeleteChild:
                    return AuthorizationRules.AuthorizeOperation.Delete;
                case Operation.Update:
                    return AuthorizationRules.AuthorizeOperation.Update;
                case Operation.UpdateChild:
                    return AuthorizationRules.AuthorizeOperation.Update;
                default:
                    break;
            }

            throw new Exception($"{operation.ToString()} cannot be converted to AuthorizationOperation");

        }
    }
  
}
