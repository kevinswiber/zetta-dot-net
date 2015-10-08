using System;

namespace Zetta.Example
{
	public class Display : Device {
		public string message;
		public readonly string type = "display";

		public Display() {
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

