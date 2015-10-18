using System;
using System.Threading.Tasks;

namespace Zetta.Core.Tests.Helpers {
    public class MockServer {
        public Func<object, Task<object>> find;
    }
}
