var os = require('os');
var path = require('path');
var spawn = require('child_process').spawn;

var isMono = (os.platform() !== 'win32');
var config = process.env.DOT_NET_CONFIG || 'Debug';
var rootPath = path.resolve(__dirname, '..');

var testRunnerPath = path.join(rootPath, 'src', 'packages', 
    'NUnit.Console.3.0.0-rc', 'tools', 'nunit3-console.exe');

var testAssemblyPath = path.join(rootPath, 'src', 'Zetta.Core.Tests',
    'bin', config, 'Zetta.Core.Tests.dll');

var file;
var args = [];

if (isMono) {
  file = 'mono';
  args.push(testRunnerPath);
  args.push(testAssemblyPath);
} else {
  file = testRunnerPath;
  args.push(testAssemblyPath);
}

spawn(file, args, { env: process.env, stdio: 'inherit' });
