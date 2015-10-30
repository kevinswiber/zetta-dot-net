using Zetta.Core;

namespace Zetta.Example {
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
        }
    }
}
