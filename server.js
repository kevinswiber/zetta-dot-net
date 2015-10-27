var path = require('path');
var zetta = require('zetta');
var DotNetScout = require('./dot_net_scout');

var assemblyFile = path.join(__dirname,
    'src', 'Zetta.Example', 'bin', 'Debug', 'Zetta.Example.dll')

zetta()
  .use(DotNetScout, { assemblyFile: assemblyFile })
  /*.use(function(server) {
    server.observe(server.where({ type: 'photocell' }), function(photocell) {
      var stream = photocell.createReadStream('intensity');
      stream.on('data', function(msg) {
        console.log('data:', msg);
      });
    });
  })*/
  .listen(process.env.PORT || 3000);
