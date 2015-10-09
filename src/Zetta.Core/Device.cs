﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

        protected Device Map(string transition, Func<Task> function, IDictionary<string, string> fields = null) {
            var value = new TransitionValue { Handler = WrapHandler(function), Fields = fields };
            _transitions.Add(transition, value);
            return this;
        }

        protected Device Map(string transition, Func<object, Task> function, IDictionary<string, string> fields = null) {
            var value = new TransitionValue { Handler = WrapHandler(function), Fields = fields };
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

        public static T Create<T>() where T : Device {
            var instance = Activator.CreateInstance<T>();
            return Device.Intercept(instance);
        }

        public static T Intercept<T>(T device) where T : Device {
            var interceptor = new PropertyInterceptor();
            //var proxy = _generator.CreateClassProxy<T>(interceptor);
            return null;
        }

        private Func<object, Task<object>> WrapHandler(Func<object, Task> function) {
            Func<object, Task<object>> wrappedHandler = async (input) => {
                await function(input);
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