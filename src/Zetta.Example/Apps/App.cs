using Zetta.Core;
using Zetta.Example.Devices;

namespace Zetta.Example.Apps {
    public class App : IApp {
        public void Invoke(Server server) {
            var queries = new[] {
                new { type = "led" },
                new { type = "photocell" }
            };

            server.Observe<LED, Photocell>(queries, async (led, photocell) => {
                var stream = await photocell.CreateReadStream("intensity");

                stream.Subscribe(async (obj) => {
                    var data = (double)obj.Data;

                    if (data > 1.0d && led.IsAvailable("turn-on")) {
                        await led.Call("turn-on");
                    } else if (data <= 1.0d && led.IsAvailable("turn-off")) {
                        await led.Call("turn-off");
                    }
                });
            });

            /*
            // Using Server.Observe(queries)

            var queries = new[] {
                new { type = "led" },
                new { type = "photocell" }
            };

            server.Observe(queries)
                .Subscribe(Observer.Create<IEnumerable<Device>>(async (devices) => {
                    var deviceArray = devices.ToArray();

                    var led = (LED)deviceArray[0];
                    var photocell = (Photocell)deviceArray[1];

                    var stream = await photocell.CreateReadStream("intensity");

                    stream.Subscribe(async (obj) => {
                        var data = (double)obj.Data;

                        if (data > 1.0d && led.IsAvailable("turn-on")) {
                            await led.Call("turn-on");
                        } else if (data <= 1.0d && led.IsAvailable("turn-off")) {
                            await led.Call("turn-off");
                        }
                    });
                }));
            */

            /*
            // Using Server.Observe(query) and Zip

            var ledQuery = new { type = "led" };
            var photocellQuery = new { type = "photocell" };

            server.Observe<LED>(ledQuery)
                .Zip(server.Observe<Photocell>(photocellQuery), (first, second) => {
                    var devices = new List<Device>();
                    devices.Add(first);
                    devices.Add(second);
                    return devices;
                })
                .Subscribe(Observer.Create<IList<Device>>(async (devices) => {
                    var led = (LED)devices[0];
                    var photocell = (Photocell)devices[1];

                    var stream = await photocell.CreateReadStream("intensity");

                    stream.Subscribe(async (obj) => {
                        var data = (double)obj.Data;

                        if (data > 1.0d && led.IsAvailable("turn-on")) {
                            await led.Call("turn-on");
                        } else if (data <= 1.0d && led.IsAvailable("turn-off")) {
                            await led.Call("turn-off");
                        }
                    });
                }));
                */
        }
    }
}
