var path = require('path');
var zetta = require('zetta');
var DotNetScout = require('./dot_net_scout');

zetta()
  .use(DotNetScout, path.join(__dirname, 'DisplayScout.csx'))
  .listen(process.env.PORT || 3000);
