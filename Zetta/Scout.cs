using System;
using System.Threading.Tasks;

namespace Zetta {
    public abstract class Scout {
        private Func<object, Task<object>> _discover;
        private Func<object, Task<object>> _provision;

        public abstract Task Initialize();

        public async Task Provision<T>(T device) where T : Device {
            await _provision(Interop.Wrap(device));
        }

        public async Task Discover<T>(T device) where T : Device {
            await _discover(Interop.Wrap(device));
        }

        public void SetProvisionFunction(Func<object, Task<object>> provision) {
            _provision = provision;
        }

        public void SetDiscoverFunction(Func<object, Task<object>> discover) {
            _discover = discover;
        }

        public Server Server { get; set; }
    }
}