using System;
using System.Threading.Tasks;
using Zetta;

namespace Zetta.Example {
	public class Startup {
		public async Task<object> Invoke(dynamic input) {
			var loader = ScoutLoader.Create(input);

			await loader.Use(new LEDScout());
			await loader.Use(new HeartbeatScout());

			return null;
		}
	}
}