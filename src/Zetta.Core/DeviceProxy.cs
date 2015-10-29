using Castle.DynamicProxy;
using Microsoft.CSharp.RuntimeBinder;

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

        public static System.Type GetInterceptedType<T>() where T : Device {
            var options = ProxyGenerationOptions.Default;
            return _generator.ProxyBuilder.CreateClassProxyType(typeof(T), null, options);
        }
        public static object InterceptDevice(System.Type type, object device) {
            if (ProxyUtil.IsProxy(device)) {
                return device;
            }

            var interceptor = new SetterInterceptor();
            var proxy = _generator.CreateClassProxyWithTarget(type, device, interceptor);
            return proxy;
        }

        public static bool IsProxy<T>(T device) {
            return ProxyUtil.IsProxy(device);
        }
    }
}