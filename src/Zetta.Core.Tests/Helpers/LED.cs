namespace Zetta.Core.Tests.Helpers {
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