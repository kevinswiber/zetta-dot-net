﻿using System.Threading.Tasks;
using Zetta.Core;

namespace Zetta.Example.Devices {
    public class Alarm : Device, IInitializableAsync {
        private Microphone _microphone;
        private Buzzer _buzzer;

        public Alarm(Microphone microphone, Buzzer buzzer) {
            _microphone = microphone;
            _buzzer = buzzer;

            State = "disarmed";

            When("disarmed", allow: "arm");
            When("armed", allow: "disarm");

            Map("arm", async () => {
                State = "armed";

                await Save();
            });

            Map("disarm", async () => {
                State = "disarmed";

                if (_buzzer.IsAvailable("stop")) {
                    await _buzzer.Call("stop");
                }

                await Save();
            });
        }

        public async Task Initialize() {
            var stream = await _microphone.CreateReadStream("volume");

            stream.Subscribe(async (obj) => {
                if (State == "disarmed") {
                    return;
                }

                var data = (double)obj.Data;

                if (data > 1.0d && _buzzer.IsAvailable("start")) {
                    await _buzzer.Call("start");
                }
            });
        }
    }
}