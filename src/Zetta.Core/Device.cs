using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace Zetta.Core {
    [JsonObject(MemberSerialization.OptOut)]
    public abstract class Device {
        [JsonIgnore]
        public IDictionary<string, string[]> _allowed = new Dictionary<string, string[]>();
        [JsonIgnore]
        public IDictionary<string, TransitionValue> _transitions = new Dictionary<string, TransitionValue>();

        private Func<object, Task<object>> _sync;
        private Func<object, Task<object>> _save;

        protected Device When(string state, string[] allow) {
            _allowed.Add(state, allow);
            return this;
        }

        protected Device When(string state, string allow) {
            return When(state, new string[] { allow });
        }

        /*protected Device MapExp<T1>(string transition, Expression<Func<T1, Task>> exp) {
            exp.Parameters.ToList().ForEach(Console.WriteLine);
            return null;
        }*/

        protected Device Map(string transition, Func<Task> function) {
            var value = new TransitionValue { Handler = WrapHandler(function), Fields = null };
            _transitions.Add(transition, value);
            return this;
        }

        protected Device Map<T1>(string transition, Func<T1, Task> function, Field field) {
            var value = 
                new TransitionValue { Handler = WrapHandler<T1>(function), Fields = new Field[] { field } };
            _transitions.Add(transition, value);
            return this;
        }

        public async Task Sync() {
            await this._sync(Interop.Wrap(this));
        }

        public async Task Save() {
            await this._save(Interop.Wrap(this));
        }

        public void SetSyncFunction(Func<object, Task<object>> function) {
            _sync = function;
        }

        public void SetSaveFunction(Func<object, Task<object>> function) {
            _save = function;
        }

        public override int GetHashCode() {
            var code = !string.IsNullOrEmpty(Id) ? Id.GetHashCode() : base.GetHashCode();
            return code;
        }

        public static T Create<T>(object[] args = null) where T : Device {
            return DeviceProxy.Create<T>(args);
        }

        private Func<object, Task<object>> WrapHandler<T1>(Func<T1, Task> function) {
            Func<object, Task<object>> wrappedHandler = async (input) => {
                var parameters = (object[])input;

                await function((T1)parameters[0]);
                return Interop.Wrap(this);
            };

            return wrappedHandler;
        }

        private Func<object, Task<object>> WrapHandler(Func<Task> function) {
            Func<object, Task<object>> wrappedHandler = async (input) => {
                await function();
                return Interop.Wrap(this);
            };

            return wrappedHandler;
        }

        [JsonProperty]
        public string Id { get; set; }

        [JsonProperty]
        public string Type { get; set; }

        [JsonProperty]
        public string State { get; set; }
    }
}