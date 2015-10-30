using Zetta.Core;

namespace Zetta.Example {
    public class Buzzer : Device {
        public Buzzer() {
            State = "silent";

            When("silent", allow: "start");
            When("ringing", allow: "stop");

            Map("start", async () => {
                State = "ringing";
                await Save();
            });

            Map("stop", async () => {
                State = "silent";
                await Save();
            });
        }
    }
}
