using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Zetta.Core.Interop;
using Zetta.Core.Interop.Commands;
using System.IO;
using System.Linq;

namespace Zetta.Core {
    [JsonObject(MemberSerialization.OptOut)]
    public abstract class Device {
        private Func<string, Task<ObjectStream>> _createReadStream;
        private Func<string, Task> _call;
        private Func<string, Task<bool>> _available;

        public Device() {
            State = null;
            Allowed = new Dictionary<string, string[]>();
            Transitions = new Dictionary<string, TransitionValue>();
        }

        protected void When(string state, string[] allow) {
            Allowed.Add(state, allow);
        }

        protected void When(string state, string allow) {
            When(state, new string[] { allow });
        }

        public void SetAvailableFunction(Func<string, Task<bool>> func) {
            _available = func;
        }

        public bool IsAvailable(string transition) {
            //return await _available.Invoke(transition);
            var isAvailable = Allowed[State].Any((availableTransition) => {
                return availableTransition == transition;
            });

            return isAvailable;
        }

        public void SetCallFunction(Func<string, Task> func) {
            _call = func;
        }

        public async Task Call(string transition) {
            await _call(transition);
        }

        public async Task Prepare() {
            await Server.Prepare(this);
        }

        public async Task Save() {
            var command = new SaveCommand(Id);
            await CommandBus.Instance.Publish(command);
        }

        public void SetCreateReadStreamFunction(Func<string, Task<ObjectStream>> func) {
            _createReadStream = func;
        }

        public Task<ObjectStream> CreateReadStream(string name) {
            return _createReadStream.Invoke(name);
        }

        public override int GetHashCode() {
            var code = !string.IsNullOrEmpty(Id) ? Id.GetHashCode() : base.GetHashCode();
            return code;
        }

        public static T Create<T>(object[] args = null) where T : Device {
            return DeviceProxy.Create<T>(args);
        }

        protected void Map(string transition, TransitionValue transitionValue) {
            Transitions.Add(transition, transitionValue);
        }

        protected void Map(string transition, Func<Task> function) {
            var value = new TransitionValue { Handler = WrapHandler(function), Fields = null };
            Transitions.Add(transition, value);
        }

        protected void Map<T1>(string transition, Func<T1, Task> function, Field field) {
            var value = 
                new TransitionValue { Handler = WrapHandler(function), Fields = new Field[] { field } };
            Transitions.Add(transition, value);
        }

        protected void Map<T1, T2>(string transition, Func<T1, T2, Task> function, IEnumerable<Field> fields) {
            var value = 
                new TransitionValue { Handler = WrapHandler(function), Fields = fields };
            Transitions.Add(transition, value);
        }

        protected void Map<T1, T2, T3>(string transition, Func<T1, T2, T3, Task> function, IEnumerable<Field> fields) {
            var value = 
                new TransitionValue { Handler = WrapHandler(function), Fields = fields };
            Transitions.Add(transition, value);
        }

        protected void Map<T1, T2, T3, T4>(string transition, Func<T1, T2, T3, T4, Task> function, IEnumerable<Field> fields) {
            var value = 
                new TransitionValue { Handler = WrapHandler(function), Fields = fields };
            Transitions.Add(transition, value);
        }

        protected void Map<T1, T2, T3, T4, T5>(string transition, Func<T1, T2, T3, T4, T5, Task> function, IEnumerable<Field> fields) {
            var value = 
                new TransitionValue { Handler = WrapHandler(function), Fields = fields };
            Transitions.Add(transition, value);
        }

        protected void Map<T1, T2, T3, T4, T5, T6>(string transition, Func<T1, T2, T3, T4, T5, T6, Task> function, IEnumerable<Field> fields) {
            var value = 
                new TransitionValue { Handler = WrapHandler(function), Fields = fields };
            Transitions.Add(transition, value);
        }

        private Func<object, Task<object>> WrapHandler(Func<Task> function) {
            Func<object, Task<object>> wrappedHandler = async (input) => {
                await function();
                return DevicePayloadFactory.Create(this);
            };

            return wrappedHandler;
        }

        private Func<object, Task<object>> WrapHandler<T1>(Func<T1, Task> function) {
            Func<object, Task<object>> wrappedHandler = async (input) => {
                var parameters = (object[])input;

                await function((T1)parameters[0]);
                return DevicePayloadFactory.Create(this);
            };

            return wrappedHandler;
        }

        private Func<object, Task<object>> WrapHandler<T1, T2>(Func<T1, T2, Task> function) {
            Func<object, Task<object>> wrappedHandler = async (input) => {
                var parameters = (object[])input;

                await function((T1)parameters[0], (T2)parameters[1]);
                return DevicePayloadFactory.Create(this);
            };

            return wrappedHandler;
        }

        private Func<object, Task<object>> WrapHandler<T1, T2, T3>(Func<T1, T2, T3, Task> function) {
            Func<object, Task<object>> wrappedHandler = async (input) => {
                var parameters = (object[])input;

                await function((T1)parameters[0], (T2)parameters[1], (T3)parameters[2]);
                return DevicePayloadFactory.Create(this);
            };

            return wrappedHandler;
        }

        private Func<object, Task<object>> WrapHandler<T1, T2, T3, T4>(Func<T1, T2, T3, T4, Task> function) {
            Func<object, Task<object>> wrappedHandler = async (input) => {
                var parameters = (object[])input;

                await function((T1)parameters[0], (T2)parameters[1], (T3)parameters[2],
                    (T4)parameters[3]);
                return DevicePayloadFactory.Create(this);
            };

            return wrappedHandler;
        }

        private Func<object, Task<object>> WrapHandler<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, Task> function) {
            Func<object, Task<object>> wrappedHandler = async (input) => {
                var parameters = (object[])input;

                await function((T1)parameters[0], (T2)parameters[1], (T3)parameters[2],
                    (T4)parameters[3], (T5)parameters[4]);
                return DevicePayloadFactory.Create(this);
            };

            return wrappedHandler;
        }

        private Func<object, Task<object>> WrapHandler<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6, Task> function) {
            Func<object, Task<object>> wrappedHandler = async (input) => {
                var parameters = (object[])input;

                await function((T1)parameters[0], (T2)parameters[1], (T3)parameters[2],
                    (T4)parameters[3], (T5)parameters[4], (T6)parameters[5]);
                return DevicePayloadFactory.Create(this);
            };

            return wrappedHandler;
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public virtual string Id { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public virtual string Name { get; set; }
        public virtual string Type { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public virtual string State { get; set; }

        [JsonIgnore]
        public IDictionary<string, string[]> Allowed { get; set; }
        [JsonIgnore]
        public IDictionary<string, TransitionValue> Transitions { get; set; }
        [JsonIgnore]
        public Server Server;
    }
}