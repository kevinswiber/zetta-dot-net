using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zetta.Core {
    public class TransitionValue {
        public Func<object, Task<object>> Handler { get; set; }

        public IEnumerable<Field> Fields { get; set; }
    }
}