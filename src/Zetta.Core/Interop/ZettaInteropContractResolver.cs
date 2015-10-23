using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Zetta.Core.Interop {
    public class ZettaInteropContractResolver : CamelCasePropertyNamesContractResolver {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization) {
            return base.CreateProperties(type, memberSerialization).Where((prop) => {
                return prop.PropertyName != "__interceptors";
            }).ToList();
        }
    }
}
