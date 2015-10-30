using System.Linq;
using System.Threading.Tasks;
using Zetta.Core;

namespace Zetta.Example {
    public class MicrophoneScout : Scout {
        public override async Task Initialize() {
            var results = await Server.Find<Microphone>("where type=\"microphone\"");

            if (results.Count() > 0) {
                await Provision(results.First());
            } else {
                await Discover(Device.Create<Microphone>());
            }
        }
    }
}