using System;
using System.Linq;
using System.Threading.Tasks;
using Zetta.Core;

namespace Zetta.Example {
    public class PhotocellScout : Scout {
        public override async Task Initialize() {
            var results = await Server.Find<Photocell>("where type=\"photocell\"");

            if (results.Count() > 0) {
                await Provision(results.First());
            } else {
                await Discover(new Photocell());
            }
        }
    }
}