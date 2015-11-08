var os = require('os');
var path = require('path');
var execSync = require('child_process').execSync;
var colors = require('colors');

var isMono = (os.platform() !== 'win32');

if (isMono) {
  console.error("Error: dot-net-coverage requires a win32 operating system".red);
  return;
}

var config = process.env.DOT_NET_CONFIG || 'Debug';
var rootPath = '.';//path.resolve(__dirname, '..');
var packagePath = path.join(rootPath, 'src', 'packages');

var coverageRunnerPath = path.join(packagePath,
    'OpenCover.4.6.166', 'tools', 'OpenCover.Console.exe');

var testRunnerPath = path.join(packagePath,
    'NUnit.Console.3.0.0-rc', 'tools', 'nunit3-console.exe');

var testAssemblyPath = path.join(rootPath, 'src', 'Zetta.Core.Tests',
    'bin', config, 'Zetta.Core.Tests.dll');

var reportGeneratorPath = path.join(packagePath, 'ReportGenerator.2.3.2.0',
    'tools', 'ReportGenerator.exe');

var file;
var args = [];

file = coverageRunnerPath;
args.push('-target:"' + testRunnerPath + '"');
args.push('-targetargs:"' + testAssemblyPath + '"');
args.push('-register:user');
args.push('-filter:"+[Zetta*]* -[*Test*]*"');

var buildPath = path.join(__dirname, 'dot-net-build.js');
execSync("node", [buildPath], { env: process.env, stdio: 'inherit' });

execSync('"' + coverageRunnerPath + '" ' + args.join(' '), { env: process.env, stdio: 'inherit' });
execSync('"' + reportGeneratorPath + '" -reports:results.xml -targetdir:.\\coverage');
execSync('start .\\coverage\\index.htm');
