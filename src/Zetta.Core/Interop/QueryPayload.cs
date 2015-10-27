using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zetta.Core.Interop {
    public class QueryPayload {
        public object Query { get; set; }
        public Func<object, Task<object>> Callback { get; set; }
    }
}
