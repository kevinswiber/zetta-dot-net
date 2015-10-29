using Castle.DynamicProxy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zetta.Core.Tests {
    public class DeviceProxyTests {
        public class Dummy : Device {
            public virtual int Value { get; set; }

            public override void Init() {
                // do nothing
            }
        }

        [Test]
        public void Create_Returns_Proxy() {
            var device = DeviceProxy.Create<Dummy>();

            Assert.That(ProxyUtil.IsProxy(device), Is.True);
        }

        [Test]
        public void Create_Associates_SetterIntercepter_With_Proxy() {
            var device = DeviceProxy.Create<Dummy>();
            var type = device.GetType();
            var interceptors = type.GetField("__interceptors").GetValue(device) as IInterceptor[];

            Assert.That(interceptors, Has.Length.EqualTo(1));
            Assert.That(interceptors[0], Is.InstanceOf<SetterInterceptor>());
        }

        [Test]
        public void InterceptDevice_Associates_SetterIntercepter_With_Proxy() {
            var device = DeviceProxy.InterceptDevice(new Dummy()) as IProxyTargetAccessor;
            var interceptors = device.GetInterceptors();

            Assert.That(interceptors, Has.Length.EqualTo(1));
            Assert.That(interceptors[0], Is.InstanceOf<SetterInterceptor>());
        }
    }
}
