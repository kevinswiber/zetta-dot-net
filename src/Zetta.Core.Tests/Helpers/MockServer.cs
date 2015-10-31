using System;
using System.Threading.Tasks;

namespace Zetta.Core.Tests.Helpers {
    public class MockServer {
        public Func<object, Task<object>> find;
        public Func<object, Task<object>> observe;
        public Func<object, Task<object>> prepare;
    }
}
