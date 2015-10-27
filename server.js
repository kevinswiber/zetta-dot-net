var path = require('path');
var zetta = require('zetta');
var DotNetScout = require('./dot_net_scout');

var assemblyFile = path.join(__dirname,
    'src', 'Zetta.Example', 'bin', 'Debug', 'Zetta.Example.dll')

zetta()
  .use(DotNetScout, { assemblyFile: assemblyFile })
  /*.use(function(server) {
    var photocellQuery = server.where({ type: 'photocell' });
    var ledQuery = server.where({ type: 'led' });
    server.observe([photocellQuery, ledQuery], function(photocell, led) {
      var stream = photocell.createReadStream('intensity');
      stream.on('data', function(msg) {
        if (msg.data > 0.5 && led.available("turn-on")) {
          led.call("turn-on");
        } else if (led.available("turn-off")) {
          led.call("turn-off");
        }
      });
    });
  })
  .use(function (server) {
    server.observe(server.where({ type: 'led' }), function (led) {
      var stream = led.createReadStream('state');
      stream.on('data', function (msg) {
        console.log(msg);
      });
    });
  })
  .use(function(server) {
    var ledQuery = server.where({ type: 'led' });
    var photocellQuery = server.where({ type: 'photocell' });
    server.observe([ledQuery, photocellQuery], function (led, photocell) {
      setInterval(function () {
        if (led.available("turn-on")) {
          led.call("turn-on");
        } else if (led.available("turn-off")) {
          led.call("turn-off");
        }
      }, 300)

      var stream = photocell.createReadStream('intensity');
      var state = 'off';
      stream.on('data', function (msg) {
        if (msg.data > 1 && state === 'off') {
          state = 'on';
          console.log('> 1, state:', state, '&& data:', msg.data);
        } else if (msg.data <= 1 && state === 'on') {
          state = 'off';
          console.log('<= 1, state:', state, '&& data:', msg.data);
        }
      })
    })
  })*/
  .listen(process.env.PORT || 3000);
