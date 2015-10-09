using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zetta.Core {
    public class TransitionValue {
        public Func<object, Task> Handler { get; set; }

        public IDictionary<string, string> Fields { get; set; }
    }
}