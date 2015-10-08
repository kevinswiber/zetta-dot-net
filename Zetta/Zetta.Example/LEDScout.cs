using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zetta;
using Newtonsoft.Json;

namespace Zetta.Example {
	public class LEDScout : Scout {
		public override async Task<object> Initialize(dynamic input) {
			var results = await Server.Find("where type=\"led\"");

			if (results.Length > 0) {
				var first = results.First() as IDictionary<string, object>;
				var led = new LED();

				led.Id = first.ContainsKey("id") ? (string)first["id"] : null;
				led.State = first.ContainsKey("state") ? (string)first["state"] : null;

				await Provision(led);
			} else {
				var led = new LED();
				await Discover(led);
			}

			return this;
		}
	}
}

