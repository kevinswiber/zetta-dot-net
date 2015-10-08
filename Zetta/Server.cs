using System;
using System.Threading.Tasks;

namespace Zetta {
	public class Server {
		public Func<object, Task<object>> find;
		public Func<object, Task<object>> observe;
	}
}

