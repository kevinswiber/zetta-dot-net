using System;
using System.Threading.Tasks;

namespace Zetta {
	public abstract class Scout {
		public Func<object, Task<object>> _discover;
		public Func<object, Task<object>> _provision;

		public abstract Task<object> Initialize(dynamic input);

		public async Task Provision<T>(T device) where T : Device {
			Wrap(device);
			await _provision(device);
		}

		public async Task Discover<T>(T device) where T : Device {
			Wrap(device);
			await _discover(device);
		}

		public void SetProvisionFunction(Func<object, Task<object>> provision) {
			_provision = provision;
		}

		public void SetDiscoverFunction(Func<object, Task<object>> discover) {
			_discover = discover;
		}

		private void Wrap<T>(T device) where T : Device {
			device.OnUpdate = async (dynamic input) => {
				device.update = (Func<object, Task<object>>)input;
				return device;
			};

			device.OnSave = async (dynamic input) => {
				device.save = (Func<object, Task<object>>)input;
				return device;
			};

			device.fetch = async (dynamic input) => {
				return device;
			};
		}

		public Server Server { get; set; }
	}
}

