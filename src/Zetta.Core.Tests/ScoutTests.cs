using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Zetta.Core.Tests.Helpers;

namespace Zetta.Core.Tests {
    [TestClass]
    public class ScoutTests {
        [TestMethod]
        public async Task Discover_Function_Is_Set() {
            var input = new MockInput();
            var server = new MockServer();

            input.discover = (obj) => {
                Assert.IsTrue(true);
                return Task.Run(() => (object)null);
            };

            server.find = (i) => {
                return Task.Run(() => (object)"[]");
            };

            input.server = server;

            var loader = ScoutLoader.Create(input);
            await loader.Use(new LEDScout());
        }

        [TestMethod]
        public async Task Provision_Function_Is_Set() {
            var input = new MockInput();
            var server = new MockServer();

            input.provision = (obj) => {
                Assert.IsTrue(true);
                return Task.Run(() => (object)null);
            };

            server.find = (i) => {
                return Task.Run(() => (object)"[{\"id\":\"1234\"}]");
            };

            input.server = server;

            var loader = ScoutLoader.Create(input);
            await loader.Use(new LEDScout());
        }

        [TestMethod]
        public async Task Discover_Ensures_Type_Is_Set() {
            var input = new MockInput();
            var server = new MockServer();

            input.discover = (obj) => {
                var interop = (Interop)obj;
                var json = JObject.Parse(interop.Properties);

                Assert.AreEqual("led", json.GetValue("type"));

                return Task.Run(() => (object)null);
            };

            server.find = (i) => {
                return Task.Run(() => (object)"[]");
            };

            input.server = server;

            var loader = ScoutLoader.Create(input);
            await loader.Use(new LEDScout());
        }

        [TestMethod]
        public async Task Provision_Ensures_Type_Is_Set() {
            var input = new MockInput();
            var server = new MockServer();

            input.provision = (obj) => {
                var interop = (Interop)obj;
                var json = JObject.Parse(interop.Properties);

                Assert.AreEqual("led", json.GetValue("type"));

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
