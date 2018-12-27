using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.Portal
{

    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class PortalOperationAttributeAttribute : Attribute
    {
        public PortalOperation Operation { get; }

        public PortalOperationAttributeAttribute(PortalOperation operation)
        {
            this.Operation = operation;
        }

    }



    public sealed class CreateAttribute : PortalOperationAttributeAttribute
    {

        public CreateAttribute() : base(PortalOperation.Create)
        {
        }

    }

    public sealed class CreateChildAttribute : PortalOperationAttributeAttribute
    {

        public CreateChildAttribute() : base(PortalOperation.CreateChild)
        {
        }

    }

    public sealed class FetchAttribute : PortalOperationAttributeAttribute
    {

        public FetchAttribute() : base(PortalOperation.Fetch)
        {
        }

    }

    public sealed class FetchChildAttribute : PortalOperationAttributeAttribute
    {

        public FetchChildAttribute() : base(PortalOperation.FetchChild)
        {
        }

    }

    public sealed class UpdateAttribute : PortalOperationAttributeAttribute
    {

        public UpdateAttribute() : base(PortalOperation.Update)
        {
        }

    }

    public sealed class UpdateChildAttribute : PortalOperationAttributeAttribute
    {

        public UpdateChildAttribute() : base(PortalOperation.UpdateChild)
        {
        }

    }

    public sealed class DeleteAttribute : PortalOperationAttributeAttribute
    {

        public DeleteAttribute() : base(PortalOperation.Delete)
        {
        }

    }

    public sealed class DeleteChildAttribute : PortalOperationAttributeAttribute
    {

        public DeleteChildAttribute() : base(PortalOperation.DeleteChild)
        {
        }

    }




}
