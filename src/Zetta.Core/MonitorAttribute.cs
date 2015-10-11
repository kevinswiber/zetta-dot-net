using System;

namespace Zetta {
    [AttributeUsage(AttributeTargets.Property)]
    public class MonitorAttribute : Attribute {
        public MonitorAttribute() {
        }
    }
}

