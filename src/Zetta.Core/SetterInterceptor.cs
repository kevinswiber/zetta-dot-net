using System;
using Castle.DynamicProxy;

namespace Zetta.Core {
    public class SetterInterceptor : IInterceptor {
        public void Intercept(IInvocation invocation) {
            if (!(invocation.InvocationTarget is Device)) {
                invocation.Proceed();
                return;
            }

            if (!invocation.Method.Name.StartsWith("set_")) {
                invocation.Proceed();
                return;
            }

            var getterName = "get_" + invocation.Method.Name.Substring(4);

            var getter = invocation.TargetType.GetMethod(getterName);

            if (getter == null) {
                invocation.Proceed();
                return;
            }

            if (!getter.IsPublic) {
                invocation.Proceed();
                return;
            }

            var device = (Device)(invocation.InvocationTarget);
            device.Sync();

            invocation.Proceed();
        }
    }
}

