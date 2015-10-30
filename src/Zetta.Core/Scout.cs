using System;
using System.Threading.Tasks;
using Zetta.Core.Interop;

namespace Zetta.Core {
    public abstract class Scout {
        private Func<object, Task<object>> _discover;
        private Func<object, Task<object>> _provision;

        public abstract Task Initialize();

        public async Task Provision<T>(T device) where T : Device {
            device.Server = Server;
            EnsureType(device);

            if (device is IInitializableAsync) {
                await ((IInitializableAsync)device).Initialize();
            } else if (device is IInitializable) {
                ((IInitializable)device).Initialize();
            }

            await _provision(DevicePayloadFactory.Create(device));
        }

        public async Task Discover<T>(T device) where T : Device {
            device.Server = Server;
            EnsureType(device);

            if (device is IInitializableAsync) {
                await ((IInitializableAsync)device).Initialize();
            } else if (device is IInitializable) {
                ((IInitializable)device).Initialize();
            }

            await _discover(DevicePayloadFactory.Create(device));
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
                device.Type = Serializer.Resolver.GetResolvedPropertyName(deviceType.Name);
            }
        }

        public Server Server { get; set; }
    }
}