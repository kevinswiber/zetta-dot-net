using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Zetta.Core.Interop;
using Zetta.Core.Tests.Helpers;

namespace Zetta.Core.Tests {
    [TestFixture]
    public class ScoutTests {
        [Test]
        public async Task Discover_Function_Is_Set() {
            var input = new MockInput();
            var server = new MockServer();

            var hasRun = false;

            input.discover = (obj) => {
                hasRun = true;
                return Task.Run(() => (object)null);
            };

            server.find = (i) => {
                return Task.Run(() => (object)"[]");
            };

            input.server = server;

            var loader = ScoutLoader.Create(input);
            await loader.Use(new LEDScout()).ContinueWith((i) => {
                Assert.That(hasRun, Is.True);
            });
        }

        [Test]
        public async Task Provision_Function_Is_Set() {
            var input = new MockInput();
            var server = new MockServer();

            var hasRun = false;

            input.provision = (obj) => {
                hasRun = true;
                return Task.Run(() => (object)null);
            };

            server.find = (i) => {
                return Task.Run(() => (object)"[{\"id\":\"1234\"}]");
            };

            input.server = server;

            var loader = ScoutLoader.Create(input);
            await loader.Use(new LEDScout()).ContinueWith((i) => {
                Assert.That(hasRun, Is.True);
            });
        }

        [Test]
        public async Task Discover_Ensures_Type_Is_Set() {
            var input = new MockInput();
            var server = new MockServer();

            input.discover = (obj) => {
                var interop = (DevicePayload)obj;
                var json = JObject.Parse(interop.Properties);

                Assert.That(json.GetValue("type").ToString(), Is.EqualTo("led"));

                return Task.Run(() => (object)null);
            };

            server.find = (i) => {
                return Task.Run(() => (object)"[]");
            };

            input.server = server;

            var loader = ScoutLoader.Create(input);
            await loader.Use(new LEDScout());
        }

        [Test]
        public async Task Provision_Ensures_Type_Is_Set() {
            var input = new MockInput();
            var server = new MockServer();

            input.provision = (obj) => {
                var interop = (DevicePayload)obj;
                var json = JObject.Parse(interop.Properties);

                Assert.That(json.GetValue("type").ToString(), Is.EqualTo("led"));

                return Task.Run(() => (object)null);
            };

            server.find = (i) => {
                return Task.Run(() => (object)"[{\"id\":\"123\"}]");
            };

            input.server = server;

            var loader = ScoutLoader.Create(input);
            await loader.Use(new LEDScout());
        }
    }
}
