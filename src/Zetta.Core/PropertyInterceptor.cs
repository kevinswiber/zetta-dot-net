using System;
using Castle.DynamicProxy;

namespace Zetta.Core {
    public class PropertyInterceptor : IInterceptor {
        public void Intercept(IInvocation invocation) {
            if (invocation.Method.Name.StartsWith("set_")) {
                if (invocation.InvocationTarget is Device) {
                    var device = (Device)(invocation.InvocationTarget);
                    device.Sync();
                }
            }
            invocation.Proceed();
        }
    }
}

