var os = require('os');
var path = require('path');
var spawnSync = require('child_process').spawnSync;

var isMono = (os.platform() !== 'win32');
var config = process.env.DOT_NET_CONFIG || 'Debug';
var rootPath = path.resolve(__dirname, '..');

var testRunnerPath = path.join(rootPath, 'src', 'packages', 
    'NUnit.Console.3.0.0-rc', 'tools', 'nunit3-console.exe');

var testAssemblyPath = path.join(rootPath, 'src', 'Zetta.Core.Tests',
    'bin', config, 'Zetta.Core.Tests.dll');

var file = isMono ? 'mono' : testRunnerPath;
var args = [];

if (isMono) {
  args.push(testRunnerPath);
}

args.push(testAssemblyPath);
args.push('--result=DotNetTestResult.xml');


var buildPath = path.join(__dirname, 'dot-net-build.js');

spawnSync('node', [buildPath], { env: process.env, stdio: 'inherit' });
spawnSync(file, args, { env: process.env, stdio: 'inherit' });
