using System;
using System.Threading.Tasks;

namespace Zetta {
    public class Server {
        private Func<object, Task<object>> _find;
        private Func<object, Task<object>> _observe;

        public void SetFindFunction(Func<object, Task<object>> find) {
            _find = find;
        }

        public void SetObserveFunction(Func<object, Task<object>> observe) {
            _observe = observe;
        }

        public async Task<dynamic[]> Find(string query) {
            return (dynamic[])await _find(query);
        }
    }
}