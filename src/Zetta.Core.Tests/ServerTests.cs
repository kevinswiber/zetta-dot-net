using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Zetta.Core.Tests.Helpers;

namespace Zetta.Core.Tests {
    [TestFixture]
    public class ServerTests {
        [Test]
        public async Task Find_Executes_Set_Function() {
            var server = new Server();
            server.SetFindFunction((input) => {
                Assert.IsTrue(true);
                return Task.Run(() => (object)"[]");
            });

            await server.Find<LED>("n/a");
        }

        [Test]
        public async Task Find_Deserializes_An_Array() {
            var server = new Server();
            server.SetFindFunction((input) => {
                return Task.Run(() => (object)"[{\"id\":\"123\",\"type\":\"led\"}]");
            });

            var results = await server.Find<LED>("n/a");

            var led = results.First();

            Assert.IsTrue(led is Device);
            Assert.AreEqual("123", led.Id);
            Assert.AreEqual("led", led.Type);
        }

        [Test]
        public async Task Find_Intercepts_Found_Devices() {
            var server = new Server();
            server.SetFindFunction((input) => {
                return Task.Run(() => (object)"[{\"id\":\"123\",\"type\":\"led\"}]");
            });

            var results = await server.Find<LED>("n/a");

            var led = results.First();

            Assert.IsTrue(DeviceProxy.IsProxy(led));
        }
    }
}