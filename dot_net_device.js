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
    .type(this._config.type)
    .state(this._config.state);

  config.allowed = this._config._allowed;

  var self = this;

  // need to account for fields, maybe through attributes
  Object.keys(this._config._transitions).forEach(function(key) {
    var handler = function(cb) {
      self._config._transitions[key].Handler(null, function(err, result) {
        result.type = result.Type;
        result.state = result.State;
        result.id = result.Id;
        result.name = result.Name;
        delete result.Type;
        delete result.State;
        delete result.Id;
        delete result.Name;

        if (err) {
          cb(err);
          return;
        }

        var reserved = ['_allowed', '_transitions', 'id', 'Id', 'update', 'OnUpdate', 'fetch', 'save', 'OnSave'];
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

    config.transitions[key] = { handler: handler, fields: self._config._transitions[key].Fields };
  });
};
