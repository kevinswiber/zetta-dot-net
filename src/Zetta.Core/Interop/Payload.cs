using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zetta.Core.Interop {
    public class Payload {
        public string Properties { get; set; }
        public Func<object, Task<object>> OnSync { get; set; }
        public Func<object, Task<object>> OnSave { get; set; }
        public Func<object, Task<object>> Fetch { get; set; }
        public Func<object, Task<object>> SetId { get; set; }
        public IDictionary<string, string[]> Allowed { get; set; }
        public IDictionary<string, TransitionValue> Transitions { get; set; }
        public IList<string> Monitors { get; set; }
    }
}
