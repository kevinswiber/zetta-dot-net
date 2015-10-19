using System.Threading.Tasks;
using Zetta.Core;

namespace Zetta.Example {
    public class Startup {
        public async Task<object> Invoke(dynamic input) {
            var loader = ScoutLoader.Create(input);

            await loader.Use(new LEDScout());
            await loader.Use(new PhotocellScout());
            await loader.Use(new DisplayScout());

            return null;
        }
    }
}