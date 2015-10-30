using System.Linq;
using System.Threading.Tasks;
using Zetta.Core;

namespace Zetta.Example {
    public class BuzzerScout : Scout {
        public override async Task Initialize() {
            var results = await Server.Find<Buzzer>("where type=\"buzzer\"");

            if (results.Count() > 0) {
                await Provision(results.First());
            } else {
                await Discover(Device.Create<Buzzer>());
            }
        }
    }
}
