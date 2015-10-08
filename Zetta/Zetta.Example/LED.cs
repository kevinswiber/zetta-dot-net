using System;
using Zetta;

namespace Zetta.Example {
	public class LED : Device {
		public readonly string type = "led";

		public LED() {
			this.allowed.Add("on", new string[] { "turn-off" });
			this.allowed.Add("off", new string[] { "turn-on" });

			this.transitions.Add("turn-on", async (input) => {
				this.state = "on";

				await this.Save();
				return this;
			});

			this.transitions.Add("turn-off", async (input) => {
				this.state = "off";

				await this.Save();
				return this;
			});

			this.state = "off";
		}
	}
}

