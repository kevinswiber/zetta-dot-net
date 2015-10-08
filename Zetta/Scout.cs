using System;
using System.Threading.Tasks;

namespace Zetta
{
	public abstract class Scout {
		public Server server;
		public Func<object, Task<object>> discover;
		public Func<object, Task<object>> provision;
		public abstract Task<object> init(dynamic input);

		public async Task Provision<T>(T device) where T : Device {
			Wrap(device);
			await this.provision(device);
		}

		public async Task Discover<T>(T device) where T : Device {
			Wrap(device);
			await this.discover(device);
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
	}
}

