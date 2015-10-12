using System;
using Zetta.Core;
using System.Threading.Tasks;

namespace Zetta.Example {
    public class LED : Device {
        public LED() {
            State = "off";

            When("on", allow: "turn-off");
            When("off", allow: "turn-on");

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