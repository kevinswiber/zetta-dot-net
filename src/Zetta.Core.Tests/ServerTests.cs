using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Zetta.Core.Tests.Helpers;

namespace Zetta.Core.Tests {
    [TestFixture]
    public class ServerTests {
        [Test]
        public async Task Find_Executes_Set_Function() {
            var hasRun = false;

            var server = new Server();
            server.SetFindFunction((input) => {
                hasRun = true;
                return Task.Run(() => (object)"[]");
            });

            await server.Find<LED>("n/a").ContinueWith((input) => {
                Assert.That(hasRun, Is.True);
            });
        }

        [Test]
        public async Task Find_Deserializes_An_Array() {
            var server = new Server();
            server.SetFindFunction((input) => {
                return Task.Run(() => (object)"[{\"id\":\"123\",\"type\":\"led\"}]");
            });

            var results = await server.Find<LED>("n/a");

            var led = results.First();

            Assert.That(led, Is.InstanceOf<Device>());
            Assert.That(led.Id, Is.EqualTo("123"));
            Assert.That(led.Type, Is.EqualTo("led"));
        }

        [Test]
        public async Task Find_Intercepts_Found_Devices() {
            var server = new Server();
            server.SetFindFunction((input) => {
                return Task.Run(() => (object)"[{\"id\":\"123\",\"type\":\"led\"}]");
            });

            var results = await server.Find<LED>("n/a");

            var led = results.First();

            Assert.That(DeviceProxy.IsProxy(led), Is.True);
        }
    }
}