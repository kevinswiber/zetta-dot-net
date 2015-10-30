using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zetta.Core;

namespace Zetta.Example {
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
