using Castle.DynamicProxy;

namespace Zetta.Core {
    public class DeviceProxy {
        private static readonly ProxyGenerator _generator = new ProxyGenerator();

        public static T Create<T>(object[] args = null) where T : Device {
            var interceptor = new SetterInterceptor();
            var proxy = (T)_generator.CreateClassProxy(typeof(T), args, interceptor);
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

        public static bool IsProxy<T>(T device) {
            return ProxyUtil.IsProxy(device);
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