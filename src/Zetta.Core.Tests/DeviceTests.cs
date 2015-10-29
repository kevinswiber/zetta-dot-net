using NUnit.Framework;
using System.Threading.Tasks;
using Zetta.Core.Interop;
using Zetta.Core.Interop.Commands;
using System;

namespace Zetta.Core.Tests {
    [TestFixture]
    public class DeviceTests {
        public class Dummy : Device {

            public Dummy() {
                State = "ready";

                When("ready", allow: new[] { "start", "stop" });
                When("start", allow: "stop");
                When("stop", allow: "start");

                Map("zero", async () => {
                    await Task.Run(() => { return "Success"; });
                });

                Map<int>("one", async (i) => {
                    await Task.Run(() => { return "Success"; });
                }, new Field { Name = "first", Type = FieldType.Number });

                Map<int, int>("two", async (i, j) => {
                    await Task.Run(() => { return "Success"; });
                }, new Field[] {
                    new Field { Name = "first", Type = FieldType.Number },
                    new Field { Name = "second", Type = FieldType.Number }
                });

                Map<int, int, int>("three", async (i, j, k) => {
                    await Task.Run(() => { return "Success"; });
                }, new Field[] {
                    new Field { Name = "first", Type = FieldType.Number },
                    new Field { Name = "second", Type = FieldType.Number },
                    new Field { Name = "third", Type = FieldType.Number }
                });

                Map<int, int, int, int>("four", async (i, j, k, l) => {
                    await Task.Run(() => { return "Success"; });
                }, new Field[] {
                    new Field { Name = "first", Type = FieldType.Number },
                    new Field { Name = "second", Type = FieldType.Number },
                    new Field { Name = "third", Type = FieldType.Number },
                    new Field { Name = "fourth", Type = FieldType.Number }
                });

                Map<int, int, int, int, int>("five", async (i, j, k, l, m) => {
                    await Task.Run(() => { return "Success"; });
                }, new Field[] {
                    new Field { Name = "first", Type = FieldType.Number },
                    new Field { Name = "second", Type = FieldType.Number },
                    new Field { Name = "third", Type = FieldType.Number },
                    new Field { Name = "fourth", Type = FieldType.Number },
                    new Field { Name = "fifth", Type = FieldType.Number }
                });

                Map<int, int, int, int, int, int>("six", async (i, j, k, l, m, n) => {
                    await Task.Run(() => { return "Success"; });
                }, new Field[] {
                    new Field { Name = "first", Type = FieldType.Number },
                    new Field { Name = "second", Type = FieldType.Number },
                    new Field { Name = "third", Type = FieldType.Number },
                    new Field { Name = "fourth", Type = FieldType.Number },
                    new Field { Name = "fifth", Type = FieldType.Number },
                    new Field { Name = "sixth", Type = FieldType.Number }
                });
            }

            public virtual int Value { get; set; }
        }

        [Test]
        public void Create_Returns_Device_Proxy() {
            var device = Device.Create<Dummy>();

            Assert.That(device, Is.InstanceOf<Dummy>());
            Assert.That(DeviceProxy.IsProxy(device), Is.True);
        }

        [Test]
        public void When_Populates_Allowed_Dictionary_With_Single_Value() {
            var device = Device.Create<Dummy>();

            Assert.That(device.Allowed, Contains.Key("start"));
            Assert.That(device.Allowed["start"], Has.Member("stop"));
        }

        [Test]
        public void When_Populates_Allowed_Dictionary_With_Multiple_Values() {
            var device = Device.Create<Dummy>();

            Assert.That(device.Allowed, Contains.Key("ready"));
            Assert.That(device.Allowed["ready"], Has.Member("start"));
            Assert.That(device.Allowed["ready"], Has.Member("stop"));
        }

        [Test]
        [Timeout(1000)]
        public async Task Save_Publishes_SaveCommand() {
            var source = new TaskCompletionSource<bool>();
            var dummy = DeviceProxy.Create<Dummy>();
            dummy.Id = "1234";

            CommandBus.Instance.Subscribe<SaveCommand>((command) => {
                var c = (SaveCommand)command;

                Assert.That(c.DeviceId, Is.EqualTo("1234"));

                CommandBus.Instance.RemoveAllSubscriptions();
                source.SetResult(true);
            });

            await dummy.Save();
            await source.Task;
        } 

        [Test]
        public void Map0_Populates_Transitions() {
            var dummy = DeviceProxy.Create<Dummy>();

            Assert.That(dummy.Transitions, Contains.Key("zero"));
            Assert.That(dummy.Transitions["zero"], Is.Not.Null);

            var transitionValue = dummy.Transitions["zero"];

            Assert.That(transitionValue.Handler, Is.Not.Null);
            Assert.That(transitionValue.Fields, Is.Null);
        }

        [Test]
        public void Map1_Populates_Transitions() {
            var dummy = DeviceProxy.Create<Dummy>();

            Assert.That(dummy.Transitions, Contains.Key("one"));
            Assert.That(dummy.Transitions["one"], Is.Not.Null);

            var transitionValue = dummy.Transitions["one"];

            Assert.That(transitionValue.Handler, Is.Not.Null);
            Assert.That(transitionValue.Fields, Has.Length.EqualTo(1));
        }

        [Test]
        public void Map2_Populates_Transitions() {
            var dummy = DeviceProxy.Create<Dummy>();

            Assert.That(dummy.Transitions, Contains.Key("two"));
            Assert.That(dummy.Transitions["two"], Is.Not.Null);

            var transitionValue = dummy.Transitions["two"];

            Assert.That(transitionValue.Handler, Is.Not.Null);
            Assert.That(transitionValue.Fields, Has.Length.EqualTo(2));
        }

        [Test]
        public void Map3_Populates_Transitions() {
            var dummy = DeviceProxy.Create<Dummy>();

            Assert.That(dummy.Transitions, Contains.Key("three"));
            Assert.That(dummy.Transitions["three"], Is.Not.Null);

            var transitionValue = dummy.Transitions["three"];

            Assert.That(transitionValue.Handler, Is.Not.Null);
            Assert.That(transitionValue.Fields, Has.Length.EqualTo(3));
        }

        [Test]
        public void Map4_Populates_Transitions() {
            var dummy = DeviceProxy.Create<Dummy>();

            Assert.That(dummy.Transitions, Contains.Key("four"));
            Assert.That(dummy.Transitions["four"], Is.Not.Null);

            var transitionValue = dummy.Transitions["four"];

            Assert.That(transitionValue.Handler, Is.Not.Null);
            Assert.That(transitionValue.Fields, Has.Length.EqualTo(4));
        }

        [Test]
        public void Map5_Populates_Transitions() {
            var dummy = DeviceProxy.Create<Dummy>();

            Assert.That(dummy.Transitions, Contains.Key("five"));
            Assert.That(dummy.Transitions["five"], Is.Not.Null);

            var transitionValue = dummy.Transitions["five"];

            Assert.That(transitionValue.Handler, Is.Not.Null);
            Assert.That(transitionValue.Fields, Has.Length.EqualTo(5));
        }

        [Test]
        public void Map6_Populates_Transitions() {
            var dummy = DeviceProxy.Create<Dummy>();

            Assert.That(dummy.Transitions, Contains.Key("six"));
            Assert.That(dummy.Transitions["six"], Is.Not.Null);

            var transitionValue = dummy.Transitions["six"];

            Assert.That(transitionValue.Handler, Is.Not.Null);
            Assert.That(transitionValue.Fields, Has.Length.EqualTo(6));
        }

        [Test]
        public void Produces_Hash_Code_Based_On_Id_Value() {
            var dummy1 = new Dummy();
            var dummy2 = new Dummy();

            dummy1.Id = "abcdefg";
            dummy2.Id = "abcdefg";

            Assert.That(dummy1.GetHashCode(), Is.EqualTo(dummy2.GetHashCode()));
        }
    }
}