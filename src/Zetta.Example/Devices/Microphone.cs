using System;
using System.Timers;
using Zetta.Core;

namespace Zetta.Example.Devices {
    public class Microphone: Device, IInitializable {
        private Timer _timer = new Timer(500);
        private int _counter = 0;

        public Microphone() {
            Volume = 0;

            _timer.Elapsed += (object sender, ElapsedEventArgs e) => {
                Volume = Math.Sin(_counter * (Math.PI / 180)) + 1.0;
                _counter += 15;
            };
        }

        public void Initialize() {
            _timer.Enabled = true;
        }

        [Monitor]
        public virtual double Volume { get; set; }
    }
}