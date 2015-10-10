using System;
using Castle.DynamicProxy;

namespace Zetta.Core {
    public class DeviceProxy {
        private static readonly ProxyGenerator _generator = new ProxyGenerator();

        public static T Create<T>() where T : Device {
            var interceptor = new SetterInterceptor();
            var proxy = _generator.CreateClassProxy<T>(interceptor);
            return proxy;
        }

        public static T InterceptDevice<T>(T device) where T : Device {
            if (ProxyUtil.IsProxy(device)) {
                return device;
            }

            var interceptor = new SetterInterceptor();
            var proxy = _generator.CreateClassProxyWithTarget<T>(device, interceptor);
            return proxy;
        }

        public static T RemoveProxy<T>(T device) where T : Device {
            if (ProxyUtil.IsProxy(device)) {
                return (T)ProxyUtil.GetUnproxiedInstance(device);
            } else {
                return device;
            }
        }
    }
}

