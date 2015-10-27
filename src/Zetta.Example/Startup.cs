using System.Threading.Tasks;
using Zetta.Core;
using Zetta.Core.Interop;

namespace Zetta.Example {
    public class Startup {
        public async Task<object> Invoke(dynamic input) {
            var loader = ScoutLoader.Create(input);
            
            await loader.Use<LEDScout>();
            await loader.Use<PhotocellScout>();
            await loader.Use<DisplayScout>();
            await loader.Use<CompositeScout>();

            return CommandBus.Instance;
        }
    }
}