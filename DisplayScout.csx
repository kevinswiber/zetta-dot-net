using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

public class Startup {
    public async Task<object> Invoke(dynamic input) {
        var server = new Server();
        server.find = (Func<object, Task<object>>)input.server.find;
        server.observe = (Func<object, Task<object>>)input.server.observe;

        var scout = new DisplayScout();
        scout.discover = (Func<object, Task<object>>)input.discover;
        scout.provision = (Func<object, Task<object>>)input.provision;
        scout.server = server;

        await scout.init(input);

        return null;
    }
}

public class Server {
    public Func<object, Task<object>> find;
    public Func<object, Task<object>> observe;
}

public interface IScout {
    Task<object> init(dynamic input);
}

public abstract class Scout : IScout {
    public Server server;
    public Func<object, Task<object>> discover;
    public Func<object, Task<object>> provision;
    public abstract Task<object> init(dynamic input);

    public async Task Provision<T>(T device) where T : Device {
        Wrap(device);
        await this.provision(device);
    }

    public async Task Discover<T>(T device) where T : Device {
        Wrap(device);
        await this.discover(device);
    }

    private void Wrap<T>(T device) where T : Device {
        device.OnUpdate = async (dynamic input) => {
            device.update = (Func<object, Task<object>>)input;
            return device;
        };

        device.OnSave = async (dynamic input) => {
            device.save = (Func<object, Task<object>>)input;
            return device;
        };

        device.fetch = async (dynamic input) => {
            return device;
        };
    }
}

public abstract class Device {
    public string id;
    public string state;

    public IDictionary<string, string[]> allowed = new Dictionary<string, string[]>();
    public IDictionary<string, Func<object, Task<object>>> transitions = new Dictionary<string, Func<object, Task<object>>>();

    public Func<object, Task<object>> fetch;
    public Func<object, Task<object>> update;
    public Func<object, Task<object>> save;

    public Func<object, Task<object>> OnUpdate;
    public Func<object, Task<object>> OnSave;

    public async Task Save() {
        await this.save(this);
        return;
    }
}

public class Display : Device {
    public string message;
    public readonly string type = "display";

    public Display() {
        this.allowed.Add("on", new string[] { "turn-off" });
        this.allowed.Add("off", new string[] { "turn-on" });

        this.transitions.Add("turn-on", async (input) => {
            this.state = "on";

            await this.Save();
            return this;
        });

        this.transitions.Add("turn-off", async (input) => {
            this.state = "off";

            await this.Save();
            return this;
        });

        this.state = "off";
    }
}

public class DisplayScout : Scout {
    public override async Task<object> init(dynamic input) {
        var results = (dynamic[])(await this.server.find("where type='display'"));

        if (results.Length > 0) {
            var first = results.First() as IDictionary<string, object>;
            var display = new Display();

            display.id = first.ContainsKey("id") ? (string)first["id"] : null;
            display.message = first.ContainsKey("message") ? (string)first["message"] : null;
            display.state = first.ContainsKey("state") ? (string)first["state"] : null;

            Console.WriteLine(first.ContainsKey("state") ? (string)first["state"] : null);

            await this.Provision(display);
        } else {
            var display = new Display();
            await this.Discover(display);
        }
        
        return input;
    }
}
