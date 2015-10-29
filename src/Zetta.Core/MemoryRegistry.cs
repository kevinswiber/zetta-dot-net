using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zetta.Core {
    public class MemoryRegistry {
        private static volatile MemoryRegistry _instance;
        private static object _syncRoot = new object();

        private readonly IDictionary<string, Device> _registry = new Dictionary<string, Device>();

        public T Get<T>(string id) where T : Device {
            return _registry[id] as T;
        }

        public object Get(string id) {
            return _registry[id];
        }

        public bool Contains<T>(T device) where T : Device {
            return _registry.Keys.Contains(device.Id);
        }

        public void Save<T>(T device) where T : Device {
            _registry.Add(device.Id, device);
        }

        public static MemoryRegistry Instance {
            get {
                if (_instance == null) {
                    lock(_syncRoot) {
                        _instance = new MemoryRegistry();
                        return _instance;
                    }
                } else {
                    return _instance;
                }
            }
        }
    }
}
