".\src\packages\OpenCover.4.6.166\tools\OpenCover.Console.exe" -target:"C:\Program Files (x86)\NUnit.org\bin\nunit-console.exe" -targetargs:".\src\Zetta.Core.Tests\bin\Debug\Zetta.Core.Tests.dll" -register:user -filter:"+[Zetta*]* -[*Test*]*"

".\src\packages\ReportGenerator.2.3.2.0\tools\ReportGenerator.exe" -reports:results.xml -targetdir:.\coverage
