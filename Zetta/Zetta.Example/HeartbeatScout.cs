using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zetta;

namespace Zetta.Example {
	public class HeartbeatScout : Scout {
		public override async Task<object> Initialize(dynamic input) {
			var results = await this.server.Find("where type=\"heartbeat\"");

			if (results.Length > 0) {
				var first = results.First() as IDictionary<string, object>;
				var heartbeat = new Heartbeat();

				heartbeat.id = first.ContainsKey("id") ? (string)first["id"] : null;
				heartbeat.pulse = first.ContainsKey("pulse") ? (int)first["pulse"] : 0;

				await this.Provision(heartbeat);
			} else {
				var heartbeat = new Heartbeat();
				await this.Discover(heartbeat);
			}

			return this;
		}
	}
}

