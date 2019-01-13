using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.Rules
{
    public class RuleProxy
    {

        public IValidateBase Target { get; set; }

        internal IPropertyAccess TargetSet => (IPropertyAccess)Target;


        internal class GetInterceptor : IInterceptor
        {
            public void Intercept(IInvocation invocation)
            {
                invocation.Proceed();
            }
        }

        internal class TimeFixSelector : IInterceptorSelector
        {
            private GetInterceptor _get = new GetInterceptor();


            public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
            {
                if (method.Name.StartsWith("get_"))
                {
                    return new IInterceptor[] { _get }.Union(interceptors).ToArray();
                }
                return interceptors;
            }
        }

        public class RuleFix
        {
            private ProxyGenerator _generator = new ProxyGenerator();
            private ProxyGenerationOptions _options = new ProxyGenerationOptions { Selector = new TimeFixSelector() };

            public IShortNameRule Fix(IValidateBase item)
            {
                return (IShortNameRule)_generator.CreateInterfaceProxyWithTarget(typeof(IValidateBase), item, _options);
            }
        }


    }
}
