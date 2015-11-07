".\src\packages\OpenCover.4.6.166\tools\OpenCover.Console.exe" -target:".\src\packages\NUnit.Console.3.0.0-rc\tools\nunit-console.exe" -targetargs:".\src\Zetta.Core.Tests\bin\Debug\Zetta.Core.Tests.dll" -register:user -filter:"+[Zetta*]* -[*Test*]*"

".\src\packages\ReportGenerator.2.3.2.0\tools\ReportGenerator.exe" -reports:results.xml -targetdir:.\coverage

start .\coverage\index.htm
