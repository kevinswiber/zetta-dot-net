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
        self.server.find(payload.query, function(err, results) {
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
      console.log(payload);
      self.discover(DotNetDevice, payload);
      callback();
    },
    provision: function(payload, callback) {
      console.log('provision');
      console.log(payload);

      var machine = self.provision(payload, DotNetDevice, payload);

      payload.OnUpdate(function(result, callback2) {
        var reserved = ['allowed', 'transitions', 'id', 'update', 'OnUpdate', 'fetch', 'save', 'OnSave'];
        Object.keys(result).forEach(function(key) {
          if (reserved.indexOf(key) === -1)  {
            machine[key] = result[key];
          }
        });

        callback2();
      });

      payload.OnSave(function(result, callback2) {
        var reserved = ['allowed', 'transitions', 'id', 'update', 'OnUpdate', 'fetch', 'save', 'OnSave'];
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
