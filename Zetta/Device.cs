using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Zetta {
	[JsonObject(MemberSerialization.OptIn)]
	public abstract class Device {
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

		protected Device Map(string transition, Func<object, Task> function, IDictionary<string, string> fields = null) {
			Func<object, Task<object>> wrappedHandler = async (input) => {
				await function(input);
				return Interop.Wrap(this);
			};

			_transitions.Add(transition, new TransitionValue { Handler = wrappedHandler, Fields = fields });
			return this;
		}

		public async Task Save() {
			await this.save(Interop.Wrap(this));
			return;
		}

		[JsonProperty]
		public string Id { get; set; }

		[JsonProperty]
		public string Type { get; set; }

		[JsonProperty]
		public string State { get; set; }
	}
}

