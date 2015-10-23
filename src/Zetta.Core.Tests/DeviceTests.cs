using NUnit.Framework;

namespace Zetta.Core.Tests {
    [TestFixture]
    public class DeviceTests {
        public class Dummy : Device {
            public virtual int Value { get; set; }
        }

        [Test]
        public void Create_Returns_Device_Proxy() {
            var device = Device.Create<Dummy>();

            Assert.That(device, Is.AssignableTo<Dummy>());
            Assert.That(DeviceProxy.IsProxy(device), Is.True);
        }
    }
}
