using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Zetta.Core.Interop;
using Zetta.Core.Interop.Commands;

namespace Zetta.Core.Tests {
    [TestFixture]
    public class SetterInterceptorTests {
        public class Dummy : Device {
            public override void Init() {
                // do nothing
            }
            public virtual int Value { get; set; }
        }

        [Test]
        [Timeout(1000)]
        public async Task Publishes_SetPropertyCommand_On_Device_Property_Set() {
            var source = new TaskCompletionSource<bool>();
            var dummy = DeviceProxy.Create<Dummy>();

            CommandBus.Instance.Subscribe<SetPropertyCommand>((command) => {
                var c = (SetPropertyCommand)command;

                Assert.That(c.PropertyName, Is.EqualTo("value"));
                Assert.That(c.PropertyValue, Is.EqualTo(3));

                source.SetResult(true);
            });

            dummy.Value = 3;

            await source.Task;
        }

        [TearDown]
        public void TearDown() {
            CommandBus.Instance.RemoveAllSubscriptions();
        }
    }
}
