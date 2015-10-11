using System;

namespace Zetta.Core {
    [AttributeUsage(AttributeTargets.Parameter)]
    public class FieldAttribute : Attribute {
        public FieldAttribute(FieldType type) {
            Type = type;
        }

        FieldType Type { get; set; }
    }
}

