using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.Portal
{

    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class OperationAttribute : Attribute
    {
        public Operation Operation { get; }

        public OperationAttribute(Operation operation)
        {
            this.Operation = operation;
        }

    }



    public sealed class CreateAttribute : OperationAttribute
    {

        public CreateAttribute() : base(Operation.Create)
        {
        }

    }

    public sealed class CreateChildAttribute : OperationAttribute
    {

        public CreateChildAttribute() : base(Operation.CreateChild)
        {
        }

    }

    public sealed class FetchAttribute : OperationAttribute
    {

        public FetchAttribute() : base(Operation.Fetch)
        {
        }

    }

    public sealed class FetchChildAttribute : OperationAttribute
    {

        public FetchChildAttribute() : base(Operation.FetchChild)
        {
        }

    }

    public sealed class UpdateAttribute : OperationAttribute
    {

        public UpdateAttribute() : base(Operation.Update)
        {
        }

    }

    public sealed class UpdateChildAttribute : OperationAttribute
    {

        public UpdateChildAttribute() : base(Operation.UpdateChild)
        {
        }

    }

    public sealed class DeleteAttribute : OperationAttribute
    {

        public DeleteAttribute() : base(Operation.Delete)
        {
        }

    }

    public sealed class DeleteChildAttribute : OperationAttribute
    {

        public DeleteChildAttribute() : base(Operation.DeleteChild)
        {
        }

    }




}
