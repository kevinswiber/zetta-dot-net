using System;
using Newtonsoft.Json.Linq;

namespace Zetta.Core {
    public class StreamMessage {
        public DateTime Date { get; set; }
        public string Topic { get; set; }
        public dynamic Data { get; set; }
    }
}
