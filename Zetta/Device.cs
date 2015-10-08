using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zetta {
	public abstract class Device {
		public string id;
		public string state;

		public IDictionary<string, string[]> allowed = new Dictionary<string, string[]>();
		public IDictionary<string, Func<object, Task<object>>> transitions = new Dictionary<string, Func<object, Task<object>>>();

		public Func<object, Task<object>> fetch;
		public Func<object, Task<object>> update;
		public Func<object, Task<object>> save;

		public Func<object, Task<object>> OnUpdate;
		public Func<object, Task<object>> OnSave;

		public async Task Save() {
			await this.save(this);
			return;
		}
	}
}

