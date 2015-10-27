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
                var state = "off";
                stream.Subscribe(async (obj) => {
                    var data = (double)obj.Data;

                    //Console.WriteLine("State: {0}", led.State);
                    //if (data > 1.0d && await led.Available("turn-on")) {
                    if (data > 1.0d && state == "off") {
                        try {
                            Console.WriteLine("Attempting to turn on");
                            state = "on";
                            Console.WriteLine("> 1.0, state is on");
                            Console.WriteLine("available? {0}", await led.Available("turn-off"));
                            await led.Call("turn-on");
                        } catch (InvalidOperationException) {
                            Console.WriteLine("Invalid operation: turn-on");
                            // do nothing
                        }
                    //} else if (data <= 1.0d && await led.Available("turn-off")) {
                    } else if (data <= 1.0d && state == "on") {
                        try {
                            Console.WriteLine("Attempting to turn off");
                            state = "off";
                            Console.WriteLine("<= 1.0, state is off");
                            Console.WriteLine("available? {0}", await led.Available("turn-on"));
                            await led.Call("turn-off");
                        } catch (InvalidOperationException) {
                            Console.WriteLine("Invalid operation: turn-off");
                            // do nothing
                        }
                    }
                });
            });

            await Task.FromResult(false);
        }
    }
}
