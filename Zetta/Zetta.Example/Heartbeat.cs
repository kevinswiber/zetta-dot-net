using System;
using System.Threading.Tasks;
using Zetta;

namespace Zetta.Example {
	public class Heartbeat : Device {

		public Heartbeat() {
			Type = "heartbeat";
			Pulse = 0;
		}

		public int Pulse { get; set; }
	}
}

