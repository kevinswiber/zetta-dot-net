using System.Threading.Tasks;
using Zetta.Core;
using Zetta.Core.Interop;
using Zetta.Example.Apps;
using Zetta.Example.Scouts;

namespace Zetta.Example {
    public class Startup {
        public async Task<object> Invoke(dynamic input) {
            await LoadScouts(input);
            LoadApps(input);

            return CommandBus.Instance;
        }

        public async Task LoadScouts(dynamic input) {
            var scoutLoader = ScoutLoader.Create(input);

            await scoutLoader.Use<LEDScout>();
            await scoutLoader.Use<PhotocellScout>();
            await scoutLoader.Use<DisplayScout>();
            await scoutLoader.Use<MicrophoneScout>();
            await scoutLoader.Use<BuzzerScout>();
            await scoutLoader.Use<AlarmScout>();

        }

        public void LoadApps(dynamic input) {
            var appLoader = AppLoader.Create(input);

            appLoader.Use<App>();
        }
    }
}