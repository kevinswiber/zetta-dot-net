var os = require('os');
var path = require('path');
var spawn = require('child_process').spawn;

var isMono = (os.platform() !== 'win32');

var file;

if (isMono) {
  file = "xbuild";
} else {
  file = 'C:\\Program Files (x86)\\MSBuild\\14.0\\Bin\\amd64\\MSBuild.exe';
}

var rootPath = path.resolve(__dirname, '..');
var solutionPath = path.join(rootPath, 'src', 'Zetta.sln');

spawn(file, [solutionPath], { env: process.env, stdio: 'inherit' });
