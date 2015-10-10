using System;
using Castle.DynamicProxy;

namespace Zetta.Core {
    public class SetterInterceptor : IInterceptor {
        public void Intercept(IInvocation invocation) {
            if (!(invocation.InvocationTarget is Device)) {
                return;
            }

            if (!invocation.Method.Name.StartsWith("set_")) {
                return;
            }

            var getterName = "get_" + invocation.Method.Name.Substring(4);

            var getter = invocation.TargetType.GetMethod(getterName);

            if (getter == null) {
                return;
            }

            if (!getter.IsPublic) {
                return;
            }

            var device = (Device)(invocation.InvocationTarget);
            device.Sync();

            invocation.Proceed();
        }
    }
}

