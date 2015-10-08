using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zetta {
	public abstract class Device {
		public class TransitionValue {
			public Func<object, Task<object>> Handler { get; set; }

			public IDictionary<string, string> Fields { get; set; }
		}

		public IDictionary<string, string[]> _allowed = new Dictionary<string, string[]>();
		public IDictionary<string, TransitionValue> _transitions = new Dictionary<string, TransitionValue>();

		public Func<object, Task<object>> fetch;
		public Func<object, Task<object>> update;
		public Func<object, Task<object>> save;

		public Func<object, Task<object>> OnUpdate;
		public Func<object, Task<object>> OnSave;

		protected Device When(string state, string[] allow) {
			_allowed.Add(state, allow);
			return this;
		}

		protected Device SetType(string type) {
			Type = type;
			return this;
		}

		protected Device SetState(string state) {
			State = state;
			return this;
		}

		protected Device Map(string transition, Func<object, Task<object>> function, IDictionary<string, string> fields = null) {
			_transitions.Add(transition, new TransitionValue { Handler = function, Fields = fields });
			return this;
		}

		public async Task Save() {
			await this.save(this);
			return;
		}

		public string Id { get; set; }

		public string Type { get; set; }

		public string State { get; set; }
	}
}

