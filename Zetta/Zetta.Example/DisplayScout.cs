using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zetta.Example
{
	public class DisplayScout : Scout {
		public override async Task<object> init(dynamic input) {
			var results = (dynamic[])(await this.server.find("where type='display'"));

			if (results.Length > 0) {
				var first = results.First() as IDictionary<string, object>;
				var display = new Display();

				display.id = first.ContainsKey("id") ? (string)first["id"] : null;
				display.message = first.ContainsKey("message") ? (string)first["message"] : null;
				display.state = first.ContainsKey("state") ? (string)first["state"] : null;

				await this.Provision(display);
			} else {
				var display = new Display();
				await this.Discover(display);
			}

			return input;
		}
	}
}

