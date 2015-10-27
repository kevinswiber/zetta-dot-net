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
                stream.Subscribe((obj) => {
                    var data = (decimal)obj.Data;

                    if (data > 1.0m) {
                        Console.WriteLine("on");
                    } else {
                        Console.WriteLine("off");
                    }
                });
            });

            await Task.FromResult(false);
        }
    }
}
