using System;
using System.Threading.Tasks;

namespace Zetta {
	public class ScoutLoader {
		private dynamic input;

		public ScoutLoader(dynamic input) {
			this.input = input;
		}

		public async Task<ScoutLoader> Use<T>(T scout) where T : Scout {
			var server = new Server();
			server.find = (Func<object, Task<object>>)input.server.find;
			server.observe = (Func<object, Task<object>>)input.server.observe;

			scout.discover = (Func<object, Task<object>>)input.discover;
			scout.provision = (Func<object, Task<object>>)input.provision;
			scout.server = server;

			await scout.Initialize(this.input);

			return this;
		}

		public static ScoutLoader Create(dynamic input) {
			return new ScoutLoader(input);
		}
	}
}

