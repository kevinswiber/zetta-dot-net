using System;
using System.Threading.Tasks;

namespace Zetta.Core {
    public class AppLoader {
        private dynamic _input;
        private Server _server;

        public AppLoader(dynamic input) {
            _input = input;
            _server = new Server();

            _server.SetFindFunction((Func<object, Task<object>>)_input.server.find);
            _server.SetObserveFunction((Func<object, Task<object>>)_input.server.observe);
            _server.SetPrepareFunction((Func<object, Task<object>>)_input.server.prepare);
        }

        public AppLoader Use<T>(T app) where T : IApp {
            app.Invoke(_server);

            return this;
        }

        public AppLoader Use<T>() where T : IApp {
            var app = Activator.CreateInstance<T>();

            Use(app);

            return this;
        }

        public AppLoader Use<T>(params object[] args) where T : IApp {
            var app = (T)Activator.CreateInstance(typeof(T), args);

            Use(app);

            return this;
        }

        public static AppLoader Create(dynamic input) {
            return new AppLoader(input);
        }
    }
}
