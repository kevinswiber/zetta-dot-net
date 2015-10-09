var util = require('util');
var Device = require('zetta-device');

var DotNetDevice = module.exports = function(config) {
  this._id = config.id;
  this._config = config;
  Device.call(this);
};
util.inherits(DotNetDevice, Device);

DotNetDevice.prototype.init = function(config) {
  config
    .type(this._config.properties.type)
    .state(this._config.properties.state);

  config.allowed = this._config.Allowed;

  this._config.Monitors.forEach(function(name) {
    config.monitor(name);
  });

  var self = this;

  // need to account for fields, maybe through attributes
  Object.keys(this._config.Transitions).forEach(function(key) {
    var handler = function(cb) {
      self._config.Transitions[key].Handler(null, function(err, result) {
        if (err) {
          cb(err);
          return;
        }

        result.properties = JSON.parse(result.Properties);

        Object.keys(result.properties).forEach(function(key) {
          if (self[key] !== result.properties[key]) {
            self[key] = result.properties[key];
          }
        });

        cb();
      });
    };

    config.transitions[key] = { handler: handler, fields: self._config.Transitions[key].Fields };
  });
};
