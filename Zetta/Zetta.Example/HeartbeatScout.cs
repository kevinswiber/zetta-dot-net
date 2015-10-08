using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zetta;

namespace Zetta.Example {
	public class HeartbeatScout : Scout {
		public override async Task<object> Initialize(dynamic input) {
			var results = await Server.Find("where type=\"heartbeat\"");

			if (results.Length > 0) {
				var first = results.First() as IDictionary<string, object>;
				var heartbeat = new Heartbeat();

				heartbeat.Id = first.ContainsKey("id") ? (string)first["id"] : null;
				heartbeat.Pulse = first.ContainsKey("pulse") ? (int)first["pulse"] : 0;

				await Provision(heartbeat);
			} else {
				var heartbeat = new Heartbeat();
				await Discover(heartbeat);
			}

			return this;
		}
	}
}

