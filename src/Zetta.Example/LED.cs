using System;
using Zetta.Core;
using System.Threading.Tasks;

namespace Zetta.Example {
    public class LED : Device {
        public LED() {
            State = "off";

            When("on", allow: new string[] { "turn-off", "test" });
            When("off", allow: new string[] { "turn-on", "test" });

            Map("turn-on", async () => {
                State = "on";
                await Save();
            });

            Map("turn-off", async () => {
                State = "off";
                await Save();
            });
        }
    }
}