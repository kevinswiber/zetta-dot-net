var path = require('path');
var zetta = require('zetta');
var DotNetScout = require('./dot_net_scout');

zetta()
  .use(DotNetScout, { assemblyFile: path.join(__dirname, 'Zetta', 'Zetta.Example', 'bin', 'Debug', 'Zetta.Example.dll') })
  .listen(process.env.PORT || 3000);
