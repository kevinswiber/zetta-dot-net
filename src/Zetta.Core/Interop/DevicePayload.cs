using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zetta.Core.Interop {
    public class DevicePayload {
        public string Properties { get; set; }
        public Func<object, Task<object>> On { get; set; }
        public Func<object, Task<object>> Prepare { get; set; }
        public Func<object, Task<object>> SetCreateReadStream { get; set; }
        public Func<object, Task<object>> SetCallFunction { get; set; }
        public Func<object, Task<object>> SetAvailableFunction { get; set; }
        public IDictionary<string, string[]> Allowed { get; set; }
        public IDictionary<string, TransitionValue> Transitions { get; set; }
        public IList<string> Monitors { get; set; }
    }
}
