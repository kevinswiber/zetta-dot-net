# zetta-dot-net

* Write [Zetta](https://github.com/zettajs/zetta) Scouts, Device Drivers, and Apps in C#
* Works cross-platform using Mono for Mac OSX and Linux support
* Runs in-process alongside Node.js using [Edge.js](https://github.com/tjanczuk/edge)

## Example

Here's a mock microphone sensor.

```c#
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
```

And here's a mock LED.

```c#
public class LED : Device {
    public LED() {
        State = "off";

        When("on", allow: "turn-off");
        When("off", allow: "turn-on");

        Map("turn-on", async () => {
            State = "on";
            await Save();
        });

        Map("turn-off", async () => {
            State = "off";
            await Save();
        });
    }
}
```

## Usage

_TODO: Document C# API_

## Install

### Runtime

Be sure to follow these instructions when setting up your environment:

* [Scripting CLR from Nodej.s: What you need](https://github.com/tjanczuk/edge#what-you-need)

### Setting up C#

Install the Zetta.js Nuget package. [TBD]

### Setting up Node.js

Run `npm install zetta-dot-net`.

Wire-up the C# assembly to Zetta.

```js
var path = require('path');
var zetta = require('zetta');
var DotNetScout = require('zetta-dot-net');

var assemblyFile = path.join(__dirname, '..',
    'src', 'Zetta.Example', 'bin', 'Debug', 'Zetta.Example.dll')

zetta()
  .use(DotNetScout, { assemblyFile: assemblyFile })
  .listen(process.env.PORT || 3000);
```

## License

MIT
