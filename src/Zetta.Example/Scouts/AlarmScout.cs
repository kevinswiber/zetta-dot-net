using System.Threading.Tasks;
using Zetta.Core;
using Zetta.Example.Devices;

namespace Zetta.Example.Scouts {
    public class AlarmScout : Scout {
        public override async Task Initialize() {
            var queries = new[] { new { type = "microphone" }, new { type = "buzzer" } };
            Server.Observe<Microphone, Buzzer>(queries, async (microphone, buzzer) => {
                var alarm = Device.Create<Alarm>(new object[] { microphone, buzzer });
                await Discover(alarm);
            });

            await Task.FromResult(0);
        }
    }
}
