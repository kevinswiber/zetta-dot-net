using System;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using Zetta.Core;

namespace Zetta.Example {
    public class Photocell : Device {
        private Timer _timer;
        private int _counter = 0;

        public Photocell() {
            Intensity = 0;

            _timer = new Timer(100);
            _timer.Enabled = true;
            _timer.Elapsed += (object sender, ElapsedEventArgs e) => {
                Intensity = Math.Sin(_counter * (Math.PI / 180)) + 1.0;
                _counter += 15;
            };
        }

        [Monitor]
        public virtual double Intensity { get; set; }
    }
}