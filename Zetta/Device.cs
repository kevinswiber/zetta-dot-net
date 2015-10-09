using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Zetta.Core {
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class Device {
        public IDictionary<string, string[]> _allowed = new Dictionary<string, string[]>();
        public IDictionary<string, TransitionValue> _transitions = new Dictionary<string, TransitionValue>();

        private Func<object, Task<object>> _update;
        private Func<object, Task<object>> _save;

        protected Device When(string state, string[] allow) {
            _allowed.Add(state, allow);
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

        public async Task Update() {
            await this._update(Interop.Wrap(this));
        }

        public async Task Save() {
            await this._save(Interop.Wrap(this));
        }

        public void SetUpdateFunction(Func<object, Task<object>> function) {
            _update = function;
        }

        public void SetSaveFunction(Func<object, Task<object>> function) {
            _save = function;
        }

        [JsonProperty]
        public string Id { get; set; }

        [JsonProperty]
        public string Type { get; set; }

        [JsonProperty]
        public string State { get; set; }
    }
}