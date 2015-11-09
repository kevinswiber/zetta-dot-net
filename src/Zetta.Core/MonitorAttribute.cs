using System;

namespace Zetta.Core {
    [AttributeUsage(AttributeTargets.Property)]
    public class MonitorAttribute : Attribute {
        public MonitorAttribute() {
        }
    }
}

