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

        protected Device Map<T1, T2>(string transition, Func<T1, T2, Task> function, IEnumerable<Field> fields) {
            var value = 
                new TransitionValue { Handler = WrapHandler<T1, T2>(function), Fields = fields };
            _transitions.Add(transition, value);
            return this;
        }

        protected Device Map<T1, T2, T3>(string transition, Func<T1, T2, T3, Task> function, IEnumerable<Field> fields) {
            var value = 
                new TransitionValue { Handler = WrapHandler<T1, T2, T3>(function), Fields = fields };
            _transitions.Add(transition, value);
            return this;
        }

        protected Device Map<T1, T2, T3, T4>(string transition, Func<T1, T2, T3, T4, Task> function, IEnumerable<Field> fields) {
            var value = 
                new TransitionValue { Handler = WrapHandler<T1, T2, T3, T4>(function), Fields = fields };
            _transitions.Add(transition, value);
            return this;
        }

        protected Device Map<T1, T2, T3, T4, T5>(string transition, Func<T1, T2, T3, T4, T5, Task> function, IEnumerable<Field> fields) {
            var value = 
                new TransitionValue { Handler = WrapHandler<T1, T2, T3, T4, T5>(function), Fields = fields };
            _transitions.Add(transition, value);
            return this;
        }

        private Func<object, Task<object>> WrapHandler(Func<Task> function) {
            Func<object, Task<object>> wrappedHandler = async (input) => {
                await function();
                return Interop.Wrap(this);
            };

            return wrappedHandler;
        }

        private Func<object, Task<object>> WrapHandler<T1>(Func<T1, Task> function) {
            Func<object, Task<object>> wrappedHandler = async (input) => {
                var parameters = (object[])input;

                await function((T1)parameters[0]);
                return Interop.Wrap(this);
            };

            return wrappedHandler;
        }

        private Func<object, Task<object>> WrapHandler<T1, T2>(Func<T1, T2, Task> function) {
            Func<object, Task<object>> wrappedHandler = async (input) => {
                var parameters = (object[])input;

                await function((T1)parameters[0], (T2)parameters[1]);
                return Interop.Wrap(this);
            };

            return wrappedHandler;
        }

        private Func<object, Task<object>> WrapHandler<T1, T2, T3>(Func<T1, T2, T3, Task> function) {
            Func<object, Task<object>> wrappedHandler = async (input) => {
                var parameters = (object[])input;

                await function((T1)parameters[0], (T2)parameters[1], (T3)parameters[2]);
                return Interop.Wrap(this);
            };

            return wrappedHandler;
        }

        private Func<object, Task<object>> WrapHandler<T1, T2, T3, T4>(Func<T1, T2, T3, T4, Task> function) {
            Func<object, Task<object>> wrappedHandler = async (input) => {
                var parameters = (object[])input;

                await function((T1)parameters[0], (T2)parameters[1], (T3)parameters[2],
                    (T4)parameters[3]);
                return Interop.Wrap(this);
            };

            return wrappedHandler;
        }

        private Func<object, Task<object>> WrapHandler<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, Task> function) {
            Func<object, Task<object>> wrappedHandler = async (input) => {
                var parameters = (object[])input;

                await function((T1)parameters[0], (T2)parameters[1], (T3)parameters[2],
                    (T4)parameters[3], (T5)parameters[4]);
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