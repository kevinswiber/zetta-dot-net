using System;
using System.Threading.Tasks;
using Zetta;
using Newtonsoft.Json;

namespace Zetta.Example {
	public class Heartbeat : Device {

		public Heartbeat() {
			Type = "heartbeat";
			Pulse = 0;
		}

		[JsonProperty]
		public int Pulse { get; set; }
	}
}

