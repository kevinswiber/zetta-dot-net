using System;
using Castle.DynamicProxy;

namespace Zetta {
    public class PropertyInterceptor : IInterceptor {
        public PropertyInterceptor() {
        }

        public void Intercept(IInvocation invocation) {
            Console.WriteLine(invocation.Method.Name);
        }
    }
}

