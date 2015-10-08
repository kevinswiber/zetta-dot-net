using System;
using System.Threading.Tasks;
using Zetta;

namespace Zetta.Example
{
	public class Startup {
		public async Task<object> Invoke(dynamic input) {
			var server = new Server();
			server.find = (Func<object, Task<object>>)input.server.find;
			server.observe = (Func<object, Task<object>>)input.server.observe;

			var scout = new DisplayScout();
			scout.discover = (Func<object, Task<object>>)input.discover;
			scout.provision = (Func<object, Task<object>>)input.provision;
			scout.server = server;

			await scout.init(input);

			return null;
		}
	}
}