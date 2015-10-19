using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zetta.Core.Interop;

namespace Zetta.Core {
    public class Server {
        private Func<object, Task<object>> _find;
        //private Func<object, Task<object>> _observe;

        public void SetFindFunction(Func<object, Task<object>> find) {
            _find = find;
        }

        /*public void SetObserveFunction(Func<object, Task<object>> observe) {
            _observe = observe;
        }*/

        public async Task<IEnumerable<T>> Find<T>(string query) where T : Device {
            var results = (string)await _find(query);
            var deserialized = Serializer.DeserializeArray<List<T>>(results)
                .Select((device) => {
                return DeviceProxy.InterceptDevice(device);
            }).ToArray();

            return deserialized.AsEnumerable();
        }
    }
}