using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Zetta.Core.Interop;
using Zetta.Core.Interop.Commands;

namespace Zetta.Core.Tests.Interop {
    [TestFixture]
    public class CommandBusTests {
        public class InteropMock {
            public string type;
            public Func<object, Task<object>> subscriber;
        }

        [Test]
        [Timeout(1000)]
        public async Task CommandBus_Publishes_To_Single_Subscriber() {
            var bus = new CommandBus();
            var source = new TaskCompletionSource<bool>();

            bus.Subscribe<SaveCommand>((cmd) => {
                var saveCommand = (SaveCommand)cmd;

                Assert.That(saveCommand.DeviceId, Is.EqualTo("1234"));
                bus.RemoveAllSubscriptions();

                source.SetResult(true);
            });

            var command = new SaveCommand("1234");
            await bus.Publish(command);

            await source.Task;
        }

        [Test]
        [Timeout(1000)]
        public async Task CommandBus_Publishes_To_Multiple_Subscribers() {
            var bus = new CommandBus();

            var source1 = new TaskCompletionSource<bool>();
            var source2 = new TaskCompletionSource<bool>();

            bus.Subscribe<SaveCommand>((cmd) => {
                var saveCommand = (SaveCommand)cmd;

                Assert.That(saveCommand.DeviceId, Is.EqualTo("1234"));

                source1.SetResult(true);
            });

            bus.Subscribe<SaveCommand>((cmd) => {
                var saveCommand = (SaveCommand)cmd;

                Assert.That(saveCommand.DeviceId, Is.EqualTo("1234"));

                source2.SetResult(true);
            });

            var command = new SaveCommand("1234");
            await bus.Publish(command);

            await Task.WhenAll(source1.Task, source2.Task).ContinueWith((obj) => {
                bus.RemoveAllSubscriptions();
            });
        }

        [Test]
        [Timeout(1000)]
        public async Task CommandBus_Subscribes_With_Command_String() {
            var bus = new CommandBus();
            var source = new TaskCompletionSource<bool>();

            bus.Subscribe("SaveCommand", (cmd) => {
                var saveCommand = (SaveCommand)cmd;

                Assert.That(saveCommand.DeviceId, Is.EqualTo("1234"));
                bus.RemoveAllSubscriptions();

                source.SetResult(true);

                return null;
            });

            var command = new SaveCommand("1234");
            await bus.Publish(command);

            await source.Task;
        }

        [Test]
        public void CommandBus_Throws_On_Invalid_Command_String() {
            Action test = () => {
                var bus = new CommandBus();
                bus.Subscribe("TestCommand", (cmd) => null);
            };

            var expectedMessage = "Command TestCommand does not exist.";
            Assert.That(new TestDelegate(test),
                Throws.Exception.With.Message.EqualTo(expectedMessage));
        }

        [Test]
        [Timeout(1000)]
        public async Task CommandBus_Subscribes_With_On() {
            var bus = new CommandBus();
            var source = new TaskCompletionSource<bool>();

            dynamic input = new InteropMock() {
                type = "SaveCommand",
                subscriber = (cmd) => {
                    var saveCommand = (SaveCommand)cmd;

                    Assert.That(saveCommand.DeviceId, Is.EqualTo("1234"));
                    bus.RemoveAllSubscriptions();

                    source.SetResult(true);
                    return null;
                }
            };

            bus.On(input);

            var command = new SaveCommand("1234");
            await bus.Publish(command);

            await source.Task;
        }
    }
}