using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Portal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ObjectPortal
{

    public interface IAdvancedObject : IValidateBase { }

    public class AdvancedObject : ValidateBase<AdvancedObject>, IAdvancedObject
    {
        public AdvancedObject(IValidateBaseServices<AdvancedObject> services, IReceivePortalChild<IAdvancedObject> constPortal) : base(services)
        {
            ConstPortal = constPortal;
        }

        public IReceivePortalChild<IAdvancedObject> ConstPortal { get; }

        [Fetch]
        public Task Fetch(IReceivePortalChild<IAdvancedObject> methodPortal)
        {
            return methodPortal.FetchChild();
        }

        [FetchChild]
        public async Task FetchChild()
        {
            await Task.Delay(10);
        }
    }

    [TestClass]
    public class PortalAdvancedTests
    {

        [TestMethod]
        public async Task PortalAdvanced_2Tier_Parrallel()
        {
            var scope = AutofacContainer.GetLifetimeScope(true);

            var portal = scope.Resolve<IReceivePortal<IAdvancedObject>>();

            Task[] tasks = new Task[4];

            tasks[0] = portal.Fetch();
            tasks[1] = portal.Fetch();
            tasks[2] = portal.Fetch();
            tasks[3] = portal.Fetch();

            await Task.WhenAll(tasks);
        }

    }
}
