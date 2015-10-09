using System;
using Zetta.Core;

namespace Zetta.Example {
    public class LED : Device {
        public LED() {
            State = "off";

            When("on", allow: new string[] { "turn-off" });
            When("off", allow: new string[] { "turn-on" });

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