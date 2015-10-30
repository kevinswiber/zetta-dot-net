using System.Threading.Tasks;
using Zetta.Core;
using Zetta.Core.Interop;

namespace Zetta.Example {
    public class Startup {
        public async Task<object> Invoke(dynamic input) {
            await AssemblyLoader.LoadFromAssembly(GetType().Assembly, input);
            return CommandBus.Instance;
        }
    }
}