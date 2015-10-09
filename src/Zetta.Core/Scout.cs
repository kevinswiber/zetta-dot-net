using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using System.Collections.Generic;

namespace Zetta.Core {
    public abstract class Scout {
        private Func<object, Task<object>> _discover;
        private Func<object, Task<object>> _provision;

        public abstract Task Initialize();

        public async Task Provision<T>(T device) where T : Device {
            EnsureType(device);
            await _provision(Interop.Wrap(device));
        }

        public async Task Discover<T>(T device) where T : Device {
            EnsureType(device);
            await _discover(Interop.Wrap(device));
        }

        public void SetProvisionFunction(Func<object, Task<object>> provision) {
            _provision = provision;
        }

        public void SetDiscoverFunction(Func<object, Task<object>> discover) {
            _discover = discover;
        }

        private void EnsureType<T>(T device) where T : Device {
            if (string.IsNullOrEmpty(device.Type)) {
                var deviceType = typeof(T);
                device.Type = Interop.Resolver.GetResolvedPropertyName(deviceType.Name);
            }
        }

        public Server Server { get; set; }
    }
}