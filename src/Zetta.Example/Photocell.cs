using System;
using System.Timers;
using Zetta.Core;

namespace Zetta.Example {
    public class Photocell : Device, IInitializable {
        private Timer _timer = new Timer(100);
        private int _counter = 0;

        public Photocell() {
            Intensity = 0;

            _timer.Elapsed += (object sender, ElapsedEventArgs e) => {
                Intensity = Math.Sin(_counter * (Math.PI / 180)) + 1.0;
                _counter += 15;
            };
        }

        public void Initialize() {
            _timer.Enabled = true;
        }

        [Monitor]
        public virtual double Intensity { get; set; }
    }
}