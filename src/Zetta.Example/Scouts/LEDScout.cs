using System.Linq;
using System.Threading.Tasks;
using Zetta.Core;
using Zetta.Example.Devices;

namespace Zetta.Example.Scouts {
    public class LEDScout : Scout {
        public override async Task Initialize() {
            var results = await Server.Find<LED>("where type=\"led\"");

            if (results.Count() > 0) {
                var first = results.First();
                await Provision(results.First());
            } else {
                await Discover(Device.Create<LED>());
            }
        }
    }
}