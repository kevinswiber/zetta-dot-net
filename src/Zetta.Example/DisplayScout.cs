using System;
using Zetta.Core;
using System.Threading.Tasks;
using System.Linq;

namespace Zetta.Example {
    public class DisplayScout : Scout {
        public override async Task Init() {
            var results = await Server.Find<Display>("where type=\"display\"");

            if (results.Count() > 0) {
                await Provision(results.First());
            } else {
                await Discover(Device.Create<Display>());
            }
        }
    }
}

