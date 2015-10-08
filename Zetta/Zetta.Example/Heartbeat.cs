using System;
using System.Threading.Tasks;
using Zetta;

namespace Zetta.Example {
	public class Heartbeat : Device {
		public readonly string type = "heartbeat";
		public int pulse = 0;

		public Heartbeat() {

		}
	}
}

