using Castle.DynamicProxy;
using Zetta.Core.Interop;
using Zetta.Core.Interop.Commands;

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

            var propertyName = invocation.Method.Name.Substring(4);
            var getterName = "get_" + propertyName;

            var getter = invocation.TargetType.GetMethod(getterName);

            if (getter == null) {
                invocation.Proceed();
                return;
            }

            if (!getter.IsPublic) {
                invocation.Proceed();
                return;
            }

            invocation.Proceed();

            var device = (Device)(invocation.InvocationTarget);

            var command = new SetPropertyCommand(device.Id, Serializer.Resolver.GetResolvedPropertyName(propertyName),
                invocation.GetArgumentValue(0));

            CommandBus.Instance.Publish(command).ContinueWith((obj) => { });
        }
    }
}
