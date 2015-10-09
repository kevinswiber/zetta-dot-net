using System;
using System.Linq;
using System.Threading.Tasks;
using Zetta.Core;

namespace Zetta.Example {
    public class LEDScout : Scout {
        public override async Task Initialize() {
            var results = await Server.Find<LED>("where type=\"led\"");

            if (results.Count() > 0) {
                await Provision(results.First());
            } else {
                await Discover(new LED());
            }
        }
    }
}