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

          callback(null, results);
        });
      },
      observe: function(payload, callback) {
      }
    },
    discover: function(payload, callback) {
      payload.type = payload.Type;
      payload.state = payload.State;
      payload.id = payload.Id;
      payload.name = payload.Name;
      delete payload.Type;
      delete payload.State;
      delete payload.Id;
      delete payload.Name;

      var machine = self.discover(DotNetDevice, payload);

      var reserved = ['_allowed', '_transitions', 'id', 'Id', 'update', 'OnUpdate', 'fetch', 'save', 'OnSave'];
      Object.keys(payload).forEach(function(key) {
        if (reserved.indexOf(key) === -1)  {
          machine[key] = payload[key];
        }
      });

      payload.OnUpdate(function(result, callback2) {
        result.type = result.Type;
        result.state = result.State;
        result.id = result.Id;
        result.name = result.Name;
        delete result.Type;
        delete result.State;
        delete result.Id;
        delete result.Name;

        var reserved = ['_allowed', '_transitions', 'id', 'Id', 'update', 'OnUpdate', 'fetch', 'save', 'OnSave'];
        Object.keys(result).forEach(function(key) {
          if (reserved.indexOf(key) === -1)  {
            machine[key] = result[key];
          }
        });

        callback2();
      });

      payload.OnSave(function(result, callback2) {
        result.type = result.Type;
        result.state = result.State;
        result.id = result.Id;
        result.name = result.Name;
        delete result.Type;
        delete result.State;
        delete result.Id;
        delete result.Name;

        var reserved = ['_allowed', '_transitions', 'id', 'Id', 'update', 'OnUpdate', 'fetch', 'save', 'OnSave'];
        Object.keys(result).forEach(function(key) {
          if (reserved.indexOf(key) === -1)  {
            machine[key] = result[key];
          }
        });

        machine.save(callback2);
      });

      callback();
    },
    provision: function(payload, callback) {
      payload.type = payload.Type;
      payload.state = payload.State;
      payload.id = payload.Id;
      payload.name = payload.Name;
      delete payload.Type;
      delete payload.State;
      delete payload.Id;
      delete payload.Name;

      var machine = self.provision({ id: payload.id, name: payload.name }, DotNetDevice, payload);

      var reserved = ['_allowed', '_transitions', 'id', 'Id', 'update', 'OnUpdate', 'fetch', 'save', 'OnSave'];
      Object.keys(payload).forEach(function(key) {
        if (reserved.indexOf(key) === -1)  {
          machine[key] = payload[key];
        }
      });

      payload.OnUpdate(function(result, callback2) {
        result.type = result.Type;
        result.state = result.State;
        result.id = result.Id;
        result.name = result.Name;
        delete result.Type;
        delete result.State;
        delete result.Id;
        delete result.Name;

        var reserved = ['_allowed', '_transitions', 'id', 'Id', 'update', 'OnUpdate', 'fetch', 'save', 'OnSave'];
        Object.keys(result).forEach(function(key) {
          if (reserved.indexOf(key) === -1)  {
            machine[key] = result[key];
          }
        });

        callback2();
      });

      payload.OnSave(function(result, callback2) {
        result.type = result.Type;
        result.state = result.State;
        result.id = result.Id;
        result.name = result.Name;
        delete result.Type;
        delete result.State;
        delete result.Id;
        delete result.Name;

        var reserved = ['_allowed', '_transitions', 'id', 'Id', 'update', 'OnUpdate', 'fetch', 'save', 'OnSave'];
        Object.keys(result).forEach(function(key) {
          if (reserved.indexOf(key) === -1)  {
            machine[key] = result[key];
          }
        });

        machine.save(callback2);
      });

      callback();
    }
  }

  this._interop(options, function(err, result) {
    if (err) {
      throw err;
    }

    next();
  });
};
