using System;
using System.Threading.Tasks;

namespace Zetta {
	public class Server {
		public Func<object, Task<object>> find;
		public Func<object, Task<object>> observe;

		public async Task<dynamic[]> Find(string query) {
			return (dynamic[])await find(query);
		}
	}
}

