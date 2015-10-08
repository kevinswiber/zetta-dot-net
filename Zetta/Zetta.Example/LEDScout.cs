using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zetta;

namespace Zetta.Example {
	public class LEDScout : Scout {
		public override async Task<object> Initialize(dynamic input) {
			var results = (dynamic[])(await this.server.find("where type=\"led\""));

			if (results.Length > 0) {
				var first = results.First() as IDictionary<string, object>;
				var led = new LED();

				led.id = first.ContainsKey("id") ? (string)first["id"] : null;
				led.state = first.ContainsKey("state") ? (string)first["state"] : null;

				await this.Provision(led);
			} else {
				var led = new LED();
				await this.Discover(led);
			}

			return input;
		}
	}
}

