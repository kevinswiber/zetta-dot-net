using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Zetta.Core;
using Zetta.Example;
using Newtonsoft.Json.Linq;

namespace Zetta.Example.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var input = new MockInput();
            var server = new MockServer();

            input.discover = (obj) => {
                Console.WriteLine("Discover");
                var interop = (Interop)obj;
                Console.WriteLine(interop.Properties);
                var json = JObject.Parse(interop.Properties);
                Console.WriteLine(json.GetValue("type"));
                return Task.Run(() => (object)null);
            };

            input.provision = (i) =>
            {
                Console.WriteLine("provision");
                return null;
            };

            server.find = (i) =>
            {
                return Task.Run(() => (object)"[]");
            };

            input.server = server;
            var startup = new Startup();
            startup.Invoke(input).Wait();
        }
    }

    public class MockInput
    {
        public Func<object, Task<object>> discover;
        public Func<object, Task<object>> provision;
        public MockServer server;
    }

    public class MockServer
    {
        public Func<object, Task<object>> find;
    }
}
