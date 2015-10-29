using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zetta.Core;

namespace Zetta.Example {
    public class CompositeScout : Scout {
        public override async Task Initialize() {
            var queries = new[] { new { type = "led" }, new { type = "photocell" } };
            Server.Observe<LED, Photocell>(queries, async (led, photocell) => {
                var stream = await photocell.CreateReadStream("intensity");

                stream.Subscribe(async (obj) => {
                    var data = (double)obj.Data;

                    if (data > 1.0d && await led.Available("turn-on")) {
                        await led.Call("turn-on");
                    } else if (data <= 1.0d && await led.Available("turn-off")) {
                        await led.Call("turn-off");
                    }
                });
            });

            await Task.FromResult(0);
        }
    }
}
