using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zetta.Core.Interop;

namespace Zetta.Core {
    public class Server {
        private Func<object, Task<object>> _find;
        private Func<object, Task<object>> _observe;
        private Func<object, Task<object>> _prepare;

        public void SetFindFunction(Func<object, Task<object>> find) {
            _find = find;
        }

        public void SetObserveFunction(Func<object, Task<object>> observe) {
            _observe = observe;
        }

        public void SetPrepareFunction(Func<object, Task<object>> prepare) {
            _prepare = prepare;
        }

        public async Task<IEnumerable<T>> Find<T>(string query) where T : Device {
            var results = (string)await _find(query);
            var deserialized = Serializer.DeserializeArray<T>(results);

            foreach (var d in deserialized) {
                if (!MemoryRegistry.Instance.Contains(d)) {
                    await Prepare(d);
                    MemoryRegistry.Instance.Save(d);
                }
            }

            return deserialized.Select((d) => (T)MemoryRegistry.Instance.Get(d.Id));
        }

        public void Observe<T>(dynamic query, Action<T> callback) where T : Device {
            var queryPayload = new QueryPayload();
            queryPayload.Query = query;
            queryPayload.Callback = (input) => {
                var devices = Serializer.DeserializeArray((string)input, new[] { typeof(IdentityDevice) }, 1).ToArray();
                var first = MemoryRegistry.Instance.Get<T>(devices[0].Id);
                callback.Invoke(first);
                return Task.FromResult<object>(null);
            };

            _observe.Invoke(queryPayload).Wait();
        }

        public class IdentityDevice : Device { }

        public void Observe<T1, T2>(dynamic query, Action<T1, T2> callback)
            where T1 : Device
            where T2 : Device {
            var queryPayload = new QueryPayload();
            queryPayload.Query = query;
            queryPayload.Callback = async (input) => {
                var devices = Serializer.DeserializeArray((string)input, new[] { typeof(IdentityDevice), typeof(IdentityDevice) }, 2).ToArray();
                var first = MemoryRegistry.Instance.Get<T1>(devices[0].Id);
                var second = MemoryRegistry.Instance.Get<T2>(devices[1].Id);
                callback.Invoke(first, second);
                return await Task.FromResult<object>(null);
            };

            _observe.Invoke(queryPayload).Wait();
        }

        public void Observe<T1, T2, T3>(dynamic query, Action<T1, T2, T3> callback)
            where T1 : Device
            where T2 : Device
            where T3 : Device {
            var queryPayload = new QueryPayload();
            queryPayload.Query = query;
            queryPayload.Callback = async (input) => {
                var devices = Serializer.DeserializeArray((string)input, new[] { typeof(IdentityDevice), typeof(IdentityDevice) }, 2).ToArray();
                var first = MemoryRegistry.Instance.Get<T1>(devices[0].Id);
                var second = MemoryRegistry.Instance.Get<T2>(devices[1].Id);
                var third = MemoryRegistry.Instance.Get<T3>(devices[2].Id);
                callback.Invoke(first, second, third);
                return await Task.FromResult<object>(null);
            };

            _observe.Invoke(queryPayload).Wait();
        }

        public void Observe<T1, T2, T3, T4>(dynamic query, Action<T1, T2, T3, T4> callback)
            where T1 : Device
            where T2 : Device
            where T3 : Device
            where T4 : Device {
            var queryPayload = new QueryPayload();
            queryPayload.Query = query;
            queryPayload.Callback = async (input) => {
                var devices = Serializer.DeserializeArray((string)input, new[] { typeof(IdentityDevice), typeof(IdentityDevice) }, 2).ToArray();
                var first = MemoryRegistry.Instance.Get<T1>(devices[0].Id);
                var second = MemoryRegistry.Instance.Get<T2>(devices[1].Id);
                var third = MemoryRegistry.Instance.Get<T3>(devices[2].Id);
                var fourth = MemoryRegistry.Instance.Get<T4>(devices[3].Id);
                callback.Invoke(first, second, third, fourth);
                return await Task.FromResult<object>(null);
            };

            _observe.Invoke(queryPayload).Wait();
        }

        public void Observe<T1, T2, T3, T4, T5>(dynamic query, Action<T1, T2, T3, T4, T5> callback)
            where T1 : Device
            where T2 : Device
            where T3 : Device
            where T4 : Device
            where T5 : Device {
            var queryPayload = new QueryPayload();
            queryPayload.Query = query;
            queryPayload.Callback = async (input) => {
                var devices = Serializer.DeserializeArray((string)input, new[] { typeof(IdentityDevice), typeof(IdentityDevice) }, 2).ToArray();
                var first = MemoryRegistry.Instance.Get<T1>(devices[0].Id);
                var second = MemoryRegistry.Instance.Get<T2>(devices[1].Id);
                var third = MemoryRegistry.Instance.Get<T3>(devices[2].Id);
                var fourth = MemoryRegistry.Instance.Get<T4>(devices[3].Id);
                var fifth = MemoryRegistry.Instance.Get<T5>(devices[4].Id);
                callback.Invoke(first, second, third, fourth, fifth);
                return await Task.FromResult<object>(null);
            };

            _observe.Invoke(queryPayload).Wait();
        }

        public void Observe<T1, T2, T3, T4, T5, T6>(dynamic query, Action<T1, T2, T3, T4, T5, T6> callback)
            where T1 : Device
            where T2 : Device
            where T3 : Device
            where T4 : Device
            where T5 : Device
            where T6 : Device {
            var queryPayload = new QueryPayload();
            queryPayload.Query = query;
            queryPayload.Callback = async (input) => {
                var devices = Serializer.DeserializeArray((string)input, new[] { typeof(IdentityDevice), typeof(IdentityDevice) }, 2).ToArray();
                var first = MemoryRegistry.Instance.Get<T1>(devices[0].Id);
                var second = MemoryRegistry.Instance.Get<T2>(devices[1].Id);
                var third = MemoryRegistry.Instance.Get<T3>(devices[2].Id);
                var fourth = MemoryRegistry.Instance.Get<T4>(devices[3].Id);
                var fifth = MemoryRegistry.Instance.Get<T5>(devices[4].Id);
                var sixth = MemoryRegistry.Instance.Get<T6>(devices[5].Id);
                callback.Invoke(first, second, third, fourth, fifth, sixth);
                return await Task.FromResult<object>(null);
            };

            _observe.Invoke(queryPayload).Wait();
        }

        public async Task Prepare<T>(T device) where T : Device {
            await _prepare.Invoke(DevicePayloadFactory.Create(device));
        }
    }
}