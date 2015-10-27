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
        var query;
        if (typeof payload.Query === 'string') {
          query = self.server.ql(payload.Query);
        } else if (Array.isArray(payload.Query)) {
          query = payload.Query.map(function(q) {
            if (typeof q === 'string') {
              return self.server.ql(q);
            } else if (typeof q === 'object') {
              return self.server.where(q);
            }
          })
        } else if (typeof payload.Query === 'object') {
          query = self.server.where(payload.Query);
        }

        self.server.observe(query, function () {
          payload.Callback(JSON.stringify(Array.prototype.slice.call(arguments).map(function (d) { return d.properties(); })), function (err) {
            if (err) {
              console.error('Error calling observe callback:', err);
            }
            // handle error;
          });
        });
        callback();
      },
      prepare: function (payload, callback) {
        payload.properties = JSON.parse(payload.Properties);
        var machine = self.server._jsDevices[payload.properties.id];

        payload.SetAvailableFunction({
          fn: function (obj, cb) {
            cb(null, machine.available(obj.transition));
          }
        }, function (err) {
          if (err) {
            console.log('error calling available:', err);
          }
        });

        payload.SetCallFunction({
          fn: function (obj, cb) {
            var transition = obj.transition;
            //console.log("NODEJS: Calling", transition);
            machine.call(transition, function (err) {
              //console.log('IN CALLBACK FOR TRANSITION CALL');
              if (err) {
                console.log(err);
              }
              cb(err);
            });
          }
        }, function(err) {
          if (err) {
            console.log('error calling call:', err);
          }
        });

        payload.SetCreateReadStream({
          fn: function (obj, cb) {
            var name = obj.name;
            var onData = obj.onData;

            setImmediate(function() {
              var stream = machine.createReadStream(name);
              stream.on('data', function (data) {
                onData(JSON.stringify(data));
              });
              stream.resume();
            });
            if (cb) {
              cb();
            }
          }, function (err) {
            if (err) {
              callback(err);
              return;
            }
            callback();
          }
        }, function (err) {
          if (err) {
            callback(err);
          } else {
            callback();
          }
        });
      }
    },
    discover: function(payload, callback) {
      payload.properties = JSON.parse(payload.Properties);

      var machine = self.discover(DotNetDevice, payload);

      Object.keys(payload.properties).forEach(function(key) {
        machine[key] = payload.properties[key];
      });

      payload.SetId(machine.id, function (err) {
        if (err) {
          callback(err);
          return;
        }

        payload.SetCreateReadStream({
          fn: function (name, onData) {
            var stream = machine.createReadStream(name);
            stream.onData(function (data) {
              onData(JSON.stringify(data));
            });
          }, function (err) {
            if (err) {
            }
          }
        }, function (err) {
          if (err) {
            callback(err);
          } else {
            callback();
          }
        });
      });

    },
    provision: function(payload, callback) {
      payload.properties = JSON.parse(payload.Properties);

      var machine = self.provision({ id: payload.properties.id }, DotNetDevice, payload);

      Object.keys(payload.properties).forEach(function(key) {
        machine[key] = payload.properties[key];
      });

      payload.SetCreateReadStream({
        fn: function (name, onData) {
          var stream = machine.createReadStream(name);
          stream.onData(function (data) {
            onData(JSON.stringify(data));
          });
        }, function (err) {
          if (err) {
            console.log('error setting create read stream function:', err);
          }
        }
      }, function (err) {
        if (err) {
          callback(err);
        } else {
          callback();
        }
      });

      //callback();
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

        if (command.PropertyName === 'state') {
          console.log('state set to:', command.PropertyValue);
        }

        callback();
      }
    }, function (err) {
      if (err) {
      }
    });

    bus.On({
      type: 'SaveCommand',
      subscriber: function (command, callback) {
        console.log('in save command');
        var device = self.server._jsDevices[command.DeviceId];
        console.log('save state set to:', device.state);
        device.save(callback);
      }
    }, function (err) {
      if (err) {
      }
    });

    next();
  });
};
