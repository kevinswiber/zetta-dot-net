using System;

namespace Zetta.Core.Interop.Commands {
    public class SetPropertyCommand : ICommand {
        public SetPropertyCommand(string deviceId, string propertyName, object propertyValue) {
            DeviceId = deviceId;
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public string DeviceId { get; private set; }
        public string PropertyName { get; private set; }
        public object PropertyValue { get; private set; }
    }
}
