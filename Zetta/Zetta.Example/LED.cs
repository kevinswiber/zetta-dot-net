using System;
using Zetta;

namespace Zetta.Example {
    public class LED : Device {
        public LED() {
            Type = "led";
            State = "off";

            When("on", allow: new string[] { "turn-off" });
            When("off", allow: new string[] { "turn-on" });

            Map("turn-on", async (input) => {
                State = "on";
                await this.Save();
            });

            Map("turn-off", async (input) => {
                State = "off";
                await this.Save();
            });
        }
    }
}