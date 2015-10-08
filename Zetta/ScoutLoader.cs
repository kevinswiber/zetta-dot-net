using System;
using System.Threading.Tasks;

namespace Zetta {
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

            _discoverFunction = (Func<object, Task<object>>)_input.discover;
            _provisionFunction = (Func<object, Task<object>>)_input.provision;
        }

        public async Task<ScoutLoader> Use<T>(T scout) where T : Scout {
            scout.SetDiscoverFunction(_discoverFunction);
            scout.SetProvisionFunction(_provisionFunction);

            scout.Server = _server;

            await scout.Initialize(_input);

            return this;
        }

        public static ScoutLoader Create(dynamic input) {
            return new ScoutLoader(input);
        }
    }
}