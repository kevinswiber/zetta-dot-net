using System.Linq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Zetta.Core.Interop;

namespace Zetta.Core.Tests.Interop {
    [TestFixture]
    public class PayloadFactoryTests {
        public class Dummy : Device {
            public Dummy() {
                Type = "dummy";
                State = "off";
                StreamingValue = 3;

                When("on", allow: "turn-off");
                When("off", allow: "turn-on");

                Map("turn-on", async () => {
                    State = "on";
                    await Save();
                });

                Map("turn-off", async () => {
                    State = "off";
                    await Save();
                });
            }

            [Monitor]
            public virtual int StreamingValue { get; set; }
        }

        [Test]
        public void Create_Includes_Property_Values_As_JSON() {
            var device = Device.Create<Dummy>();
            var payload = PayloadFactory.Create(device);

            var properties = JObject.Parse(payload.Properties).Properties().ToArray();
            var names = properties.Select((p) => p.Name).ToArray();

            Assert.That(names, Has.Member("type"));
            Assert.That(names, Has.Member("state"));
            Assert.That(names, Has.Member("streamingValue"));

            var type = properties
                .Where((p) => p.Name == "type").First();

            var state = properties
                .Where((p) => p.Name == "state").First();

            var streamingValue = properties
                .Where((p) => p.Name == "streamingValue").First();

            Assert.That((string)type, Is.EqualTo("dummy"));
            Assert.That((string)state, Is.EqualTo("off"));
            Assert.That((int)streamingValue.Value, Is.EqualTo(3));
        }

        [Test]
        public void Create_Includes_Allowed_Dictionary() {
            var device = Device.Create<Dummy>();
            var payload = PayloadFactory.Create(device);

            Assert.That(payload.Allowed.Keys, Has.Member("on"));
            Assert.That(payload.Allowed.Keys, Has.Member("off"));
            Assert.That(payload.Allowed["on"], Has.Member("turn-off"));
            Assert.That(payload.Allowed["off"], Has.Member("turn-on"));
        }

        [Test]
        public void Create_Includes_Transitions_Dictionary() {
            var device = Device.Create<Dummy>();
            var payload = PayloadFactory.Create(device);

            Assert.That(payload.Transitions.Keys, Has.Member("turn-on"));
            Assert.That(payload.Transitions.Keys, Has.Member("turn-off"));
            Assert.That(payload.Transitions["turn-on"], Is.Not.Null);
            Assert.That(payload.Transitions["turn-off"], Is.Not.Null);
        }

        [Test]
        public void Create_Includes_Monitor_Properties() {
            var device = Device.Create<Dummy>();
            var payload = PayloadFactory.Create(device);

            Assert.That(payload.Monitors.ToArray(), Has.Length.EqualTo(1));
            Assert.That(payload.Monitors.First(), Is.EqualTo("streamingValue"));
        }
    }
}
