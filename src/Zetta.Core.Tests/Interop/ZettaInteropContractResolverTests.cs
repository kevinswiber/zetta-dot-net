using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zetta.Core.Interop;
using Zetta.Core.Tests.Helpers;

namespace Zetta.Core.Tests.Interop {
    [TestFixture]
    public class ZettaInteropContractResolverTests {
        [Test]
        public void Resolver_Removes_Proxy_Interceptors() {
            var resolver = new ZettaInteropContractResolver();
            var device = Device.Create<LED>();

            var settings = new JsonSerializerSettings();
            settings.ContractResolver = resolver;

            var json = JsonConvert.SerializeObject(device, settings);

            var properties = JObject.Parse(json).Properties()
                .Select((p) => p.Name).ToArray();

            Assert.That(DeviceProxy.IsProxy(device), Is.True);
            Assert.That(properties, Has.No.Member("__interceptors"));
        }
    }
}
