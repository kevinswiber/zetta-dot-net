using System.Threading.Tasks;
using NUnit.Framework;
using Zetta.Core.Tests.Helpers;

namespace Zetta.Core.Tests {
    [TestFixture]
    public class ScoutLoaderTests {
        [Test]
        public async Task Use_Sets_Discover_Function_From_Loader() {
            var input = new MockInput();
            var server = new MockServer();

            var hasRun = false;

            server.find = (i) => {
                return Task.Run(() => (object)"[]");
            };

            input.server = server;
            input.discover = (obj) => Task.Run(() => {
                hasRun = true;
                return (object)null;
            });
            input.provision = (obj) => Task.Run(() => { return (object)null; });

            var loader = ScoutLoader.Create(input);
            var scout = new LEDScout();
            await loader.Use(scout);

            await scout.Discover(Device.Create<LED>()).ContinueWith((obj) => {
                Assert.That(hasRun, Is.True);
            });
        }

        [Test]
        public async Task Use_Sets_Provision_Function_From_Loader() {
            var input = new MockInput();
            var server = new MockServer();

            var hasRun = false;

            server.find = (i) => {
                return Task.Run(() => (object)"[{\"id\":\"1234\"}]");
            };

            input.server = server;
            input.provision = (obj) => Task.Run(() => {
                hasRun = true;
                return (object)null;
            });
            input.discover = (obj) => Task.Run(() => { return (object)null; });

            var loader = ScoutLoader.Create(input);
            var scout = new LEDScout();
            await loader.Use(scout);

            await scout.Provision(Device.Create<LED>()).ContinueWith((obj) => {
                Assert.That(hasRun, Is.True);
            });
        }
    }
}
