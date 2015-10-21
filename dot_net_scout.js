var util = require('util');
var edge = require('edge');
var Scout = require('zetta-scout');
var DotNetDevice = require('./dot_net_device');

var DotNetScout = module.exports = function(fileName) {
  Scout.call(this);

  this._interop = edge.func(fileName);
};
util.inherits(DotNetScout, Scout);

DotNetScout.prototype.init = function(next) {
  var self = this;
  var options = {
    server: {
      find: function(payload, callback) {
        self.server.find(payload, function(err, results) {
          if (err) {
            callback(err);
            return;
          } 

          callback(null, JSON.stringify(results));
        });
      },
      observe: function(payload, callback) {
      }
    },
    discover: function(payload, callback) {
      payload.properties = JSON.parse(payload.Properties);
      if (payload.properties.name === null) {
        delete payload.properties.name;
      }
      if (payload.properties.id === null) {
        delete payload.properties.id;
      }

      var machine = self.discover(DotNetDevice, payload);

      Object.keys(payload.properties).forEach(function(key) {
        machine[key] = payload.properties[key];
      });

      payload.OnSync(function(result, callback2) {
        result.properties = JSON.parse(result.Properties);
        Object.keys(result.properties).forEach(function(key) {
          machine[key] = result.properties[key];
        });

        callback2();
      });

      payload.OnSave(function(result, callback2) {
        result.properties = JSON.parse(result.Properties);

        Object.keys(result.properties).forEach(function(key) {
          machine[key] = result.properties[key];
        });

        machine.save(callback2);
      });

      payload.SetId(machine.id, callback);
    },
    provision: function(payload, callback) {
      payload.properties = JSON.parse(payload.Properties);
      if (payload.properties.name === null) {
        delete payload.properties.name;
      }
      if (payload.properties.id === null) {
        delete payload.properties.id;
      }

      var machine = self.provision({ id: payload.properties.id }, DotNetDevice, payload);

      Object.keys(payload.properties).forEach(function(key) {
        machine[key] = payload.properties[key];
      });

      payload.OnSync(function(result, callback2) {
        result.properties = JSON.parse(result.Properties);

        Object.keys(result.properties).forEach(function(key) {
          machine[key] = result.properties[key];
        });

        callback2();
      });

      payload.OnSave(function(result, callback2) {
        result.properties = JSON.parse(result.Properties);

        Object.keys(result.properties).forEach(function(key) {
          machine[key] = result.properties[key];
        });

        machine.save(callback2);
      });

      callback();
    }
  }

  this._interop(options, function(err, bus) {
    if (err) {
      console.log(err);
    }

    // wire up command bus
    bus.On({
      type: 'SetPropertyCommand',
      subscriber: function(command, callback) {
        var device = self.server._jsDevices[command.DeviceId];
        device[command.PropertyName] = command.PropertyValue;

        callback();
      }
    }, function (err) {
      if (err) {
      }
    });

    bus.On({
      type: 'SaveCommand',
      subscriber: function (command, callback) {
        var device = self.server._jsDevices[command.DeviceId];
        device.save(callback);
      }
    }, function (err) {
      if (err) {
      }
    });

    next();
  });
};
