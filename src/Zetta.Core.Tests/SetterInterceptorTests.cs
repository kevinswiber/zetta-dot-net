using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zetta.Core.Tests {
    [TestFixture]
    public class SetterInterceptorTests {
        public class Dummy : Device {
            public virtual int Value { get; set; }
        }

        [Test]
        [Timeout(1000)]
        public async Task Fires_Sync_Function_On_Device_Property_Set() {
            var source = new TaskCompletionSource<bool>();
            var dummy = DeviceProxy.Create<Dummy>();

            dummy.SetSyncFunction((input) => {
                var device = Interop.Deserialize<Dummy>(((Interop)input).Properties);

                Assert.AreEqual(3, device.Value);
                source.SetResult(true);

                return null;
            });

            dummy.Value = 3;

            await source.Task;
        }
    }
}
