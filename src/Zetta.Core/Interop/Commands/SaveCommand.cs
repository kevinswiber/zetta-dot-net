namespace Zetta.Core.Interop.Commands {
    public class SaveCommand : ICommand {
        public SaveCommand(string deviceId) {
            DeviceId = deviceId;
        }

        public string DeviceId { get; set; }
    }
}
