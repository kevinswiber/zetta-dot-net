using System;
using System.Threading.Tasks;

namespace Zetta.Core {
    public class ScoutLoader {
        private dynamic _input;
        private Server _server;
        private Func<object, Task<object>> _discoverFunction;
        private Func<object, Task<object>> _provisionFunction;

        public ScoutLoader(dynamic input) {
            _input = input;
            _server = new Server();

            _server.SetFindFunction((Func<object, Task<object>>)_input.server.find);
            _server.SetObserveFunction((Func<object, Task<object>>)_input.server.observe);
            _server.SetPrepareFunction((Func<object, Task<object>>)_input.server.prepare);

            _discoverFunction = (Func<object, Task<object>>)_input.discover;
            _provisionFunction = (Func<object, Task<object>>)_input.provision;
        }

        public async Task<ScoutLoader> Use<T>(T scout) where T : Scout {
            Setup(scout);

            await scout.Initialize();

            return this;
        }

        public async Task<ScoutLoader> Use<T>() where T : Scout {
            var scout = Activator.CreateInstance<T>();

            await Use(scout);

            return this;
        }

        public async Task<ScoutLoader> Use<T>(params object[] args) where T : Scout {
            var scout = (T)Activator.CreateInstance(typeof(T), args);

            await Use(scout);

            return this;
        }

        public static ScoutLoader Create(dynamic input) {
            return new ScoutLoader(input);
        }

        private void Setup(Scout scout) {
            scout.SetDiscoverFunction(_discoverFunction);
            scout.SetProvisionFunction(_provisionFunction);

            scout.Server = _server;
        }
    }
}