var util = require('util');
var Device = require('zetta-device');

var DotNetDevice = module.exports = function(config) {
  this._id = config.id;
  this._config = config;
  Device.call(this);
  /*this._config.update(function(err, result) {
    var reserved = ['allowed', 'transitions', 'id'];
    Object.keys(result).forEach(function(key) {
      self[key] = result[key];
    });
  }, function(err, callback) {
    console.log('done update');
  });*/
};
util.inherits(DotNetDevice, Device);

DotNetDevice.prototype.init = function(config) {
  config
    .type(this._config.type)
    .state(this._config.state);

  config.allowed = this._config.allowed;

  var self = this;

  // need to account for fields, maybe through attributes
  Object.keys(this._config.transitions).forEach(function(key) {
    var handler = function(cb) {
      self._config.transitions[key](null, function(err, result) {
        if (err) {
          cb(err);
          return;
        }

        var reserved = ['allowed', 'transitions', 'id', 'update', 'OnUpdate', 'fetch', 'save', 'OnSave'];
        Object.keys(result).forEach(function(key) {
          if (reserved.indexOf(key) === -1)  {
            if (self[key] !== result[key]) {
              self[key] = result[key];
            }
          }
        });

        cb();
      });
    };

    config.transitions[key] = { handler: handler };
  });
};
