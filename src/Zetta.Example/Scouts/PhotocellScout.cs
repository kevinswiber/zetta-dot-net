using System.Linq;
using System.Threading.Tasks;
using Zetta.Core;
using Zetta.Example.Devices;

namespace Zetta.Example.Scouts {
    public class PhotocellScout : Scout {
        public override async Task Initialize() {
            var results = await Server.Find<Photocell>("where type=\"photocell\"");

            if (results.Count() > 0) {
                await Provision(results.First());
            } else {
                await Discover(Device.Create<Photocell>());
            }
        }
    }
}