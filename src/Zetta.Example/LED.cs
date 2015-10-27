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
                Console.WriteLine("Changing state to on");
                State = "on";
                await Save();
            });

            Map("turn-off", async () => {
                Console.WriteLine("Changing state to off");
                State = "off";
                await Save();
            });
        }
    }
}