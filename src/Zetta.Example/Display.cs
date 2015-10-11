using System;
using Zetta.Core;

namespace Zetta.Example {
    public class Display : Device {
        public Display() {
            State = "ready";

            When("ready", allow: "change");

            Map<string>("change", async (message) => {
                Message = message;
                await Save();
            }, new Field { Name = "message", Type = FieldType.Text });
        }

        [Monitor]
        public string Message { get; set; }
    }
}

