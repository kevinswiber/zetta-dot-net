using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zetta.Core.Interop {
    public class DeviceConverter<T> : CustomCreationConverter<T> 
        where T : Device {
        public override T Create(Type objectType) {
            return DeviceProxy.Create<T>();
        }
    }
}
