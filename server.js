var path = require('path');
var zetta = require('zetta');
var DotNetScout = require('./dot_net_scout');

var assemblyFile = path.join(__dirname,
    'Zetta', 'Zetta.Example', 'bin', 'Debug', 'Zetta.Example.dll')

zetta()
  .use(DotNetScout, { assemblyFile: assemblyFile })
  .listen(process.env.PORT || 3000);
