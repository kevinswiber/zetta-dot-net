using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Zetta.Core.Interop;

namespace Zetta.Core.Tests.Interop {
    [TestFixture]
    public class SerializerTests {
        public class Dummy : Device {
            public override void Init() {
                Type = "dummy";
            }

            public virtual int? ValueItem { get; set; }
        }

        [Test]
        public void Serialize_Removes_Proxy_Interceptors_Field() {
            var device = Device.Create<Dummy>();
            var json = Serializer.Serialize(device);

            var obj = JObject.Parse(json);

            var properties = obj.Properties().Select((p) => p.Name).ToArray();

            Assert.That(properties, Has.No.Member("__interceptors"));
        }

        [Test]
        public void Serialize_Uses_Camel_Case_Property_Names() {
            var device = Device.Create<Dummy>();
            var json = Serializer.Serialize(device);

            var obj = JObject.Parse(json);

            var properties = obj.Properties().Select((p) => p.Name).ToArray();

            Assert.That(properties, Has.Length.EqualTo(2)); // Type, Value
            Assert.That(properties, Has.Member("valueItem"));
            Assert.That(properties, Has.Member("type"));
        }

        [Test]
        public void Serialize_Removes_Null_State_Property() {
            var device = Device.Create<Dummy>();
            device.State = null;

            var json = Serializer.Serialize(device);

            var obj = JObject.Parse(json);

            var properties = obj.Properties().Select((p) => p.Name).ToArray();

            Assert.That(properties, Has.No.Member("state"));
        }

        [Test]
        public void Serialize_Removes_Null_Id_Property() {
            var device = Device.Create<Dummy>();
            device.Id = null;

            var json = Serializer.Serialize(device);

            var obj = JObject.Parse(json);

            var properties = obj.Properties().Select((p) => p.Name).ToArray();

            Assert.That(properties, Has.No.Member("id"));
        }

        [Test]
        public void Serialize_Removes_Null_Name_Property() {
            var device = Device.Create<Dummy>();
            device.Id = null;

            var json = Serializer.Serialize(device);

            var obj = JObject.Parse(json);

            var properties = obj.Properties().Select((p) => p.Name).ToArray();

            Assert.That(properties, Has.No.Member("name"));
        }

        [Test]
        public void Serialize_Preserves_Null_Valued_Custom_Properties() {
            var device = Device.Create<Dummy>();
            device.ValueItem = null;

            var json = Serializer.Serialize(device);

            var obj = JObject.Parse(json);

            var property = obj.Properties()
                .Where((p) => p.Name == "valueItem").First();

            Assert.That(property.Value.Type, Is.EqualTo(JTokenType.Null));
        }

        [Test]
        public void DeserializeArray_Converts_JSON_To_Devices() {
            var device1 = Device.Create<Dummy>();
            var device2 = Device.Create<Dummy>();

            device1.ValueItem = 1;
            device2.ValueItem = 2;

            var json = "[" + Serializer.Serialize(device1) + "," +
                Serializer.Serialize(device2) + "]";

            var arr = Serializer.DeserializeArray<Dummy>(json).ToArray();

            Assert.That(arr[0].ValueItem, Is.EqualTo(1));
            Assert.That(arr[1].ValueItem, Is.EqualTo(2));
            Assert.That(arr[0], Is.InstanceOf<Dummy>());
            Assert.That(arr[1], Is.InstanceOf<Dummy>());
        }

        [Test]
        public void DeserializeArray_Preserves_Null_Values() {
            var device1 = Device.Create<Dummy>();

            device1.ValueItem = null;

            var json = "[" + Serializer.Serialize(device1) + "]";

            var arr = Serializer.DeserializeArray<Dummy>(json).ToArray();

            Assert.That(arr[0].ValueItem, Is.Null);
        }
    }
}
