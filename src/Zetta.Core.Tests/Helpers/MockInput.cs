using System;
using System.Threading.Tasks;

namespace Zetta.Core.Tests.Helpers {
    public class MockInput {
        public Func<object, Task<object>> discover;
        public Func<object, Task<object>> provision;
        public MockServer server;
    }
}
