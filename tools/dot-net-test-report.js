var os = require('os');
var path = require('path');
var execSync = require('child_process').execSync;
var spawnSync = require('child_process').spawnSync;

var platform = os.platform();
var isMono = (platform !== 'win32');
var config = process.env.DOT_NET_CONFIG || 'Debug';
var rootPath = path.resolve(__dirname, '..');

var reportRunnerPath = path.join(rootPath, 'src', 'packages', 
    'ReportUnit.1.2.1', 'tools', 'ReportUnit.exe');

var file = isMono ? 'mono' : reportRunnerPath;
var args = [];

if (isMono) {
  args.push(reportRunnerPath);
}

args.push('DotNetTestResult.xml');
args.push('DotNetTestResult.html');

var testRunnerPath = path.join(__dirname, 'dot-net-test.js');

spawnSync('node', [testRunnerPath], { env: process.env, stdio: 'inherit' });
spawnSync(file, args, { env: process.env, stdio: 'inherit' });

if (platform === 'darwin') {
  spawnSync('open', ['DotNetTestResult.html']);
} else if (platform === 'win32') {
console.log('spawning start');
  execSync('start DotNetTestResult.html');
}
