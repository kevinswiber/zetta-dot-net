using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zetta.Core.Interop.Commands;

namespace Zetta.Core.Interop {
    public  class CommandBus {
        private static volatile CommandBus _instance;
        private static object _syncRoot = new object();

        private readonly IDictionary<string, Type> _typeLookup = new Dictionary<string, Type>();
        private IDictionary<Type, IList<Action<ICommand>>> _subscribers =
            new ConcurrentDictionary<Type, IList<Action<ICommand>>>();

        public Func<object, Task<object>> On;

        public CommandBus() {
            _typeLookup.Add("SetPropertyCommand", typeof(SetPropertyCommand));
            _typeLookup.Add("SaveCommand", typeof(SaveCommand));

            On = (dynamic input) => {
                Subscribe((string)input.type, (Func<object, Task<object>>)input.subscriber);
                return Task.Run<object>(() => { return null; });
            };
        }

        public void Subscribe<T>(Action<ICommand> subscriber) where T : ICommand {
            if (!_subscribers.ContainsKey(typeof(T))) {
                _subscribers.Add(typeof(T), new List<Action<ICommand>>());
            }

            _subscribers[typeof(T)].Add(subscriber);
        }

        public void Subscribe(string commandType, Func<object, Task<object>> subscriber) {
            if (!_typeLookup.ContainsKey(commandType)) {
                throw new ArgumentException("Command " + commandType + " does not exist.");
            }

            var type = _typeLookup[commandType];
            if (!_subscribers.ContainsKey(type)) {
                _subscribers.Add(type, new List<Action<ICommand>>());
            }

            Action<ICommand> action = (command) => {
                subscriber.Invoke(command);
            };

            _subscribers[type].Add(action);
        }

        public async Task Publish<T>(T command) where T : ICommand {
            if (!_subscribers.ContainsKey(typeof(T))) {
                return;
            }

            var subscribers = _subscribers[typeof(T)]
                .Select((subscriber) => new Task(() => subscriber.Invoke(command)))
                .ToList();

            subscribers.ForEach((task) => task.Start());

            await Task.WhenAll(subscribers);
        }

        public void RemoveAllSubscriptions() {
            _subscribers = new ConcurrentDictionary<Type, IList<Action<ICommand>>>();
        }

        public static CommandBus Instance {
            get {
                if (_instance == null) {
                    lock(_syncRoot) {
                        _instance = new CommandBus();
                        return _instance;
                    }
                } else {
                    return _instance;
                }
            }
        }
    }
}
