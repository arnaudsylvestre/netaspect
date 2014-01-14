"C:\Program Files (x86)\OpenCover\OpenCover.Console.exe"  -target:"lib\NUnit-2.6.3\bin\nunit-console-x86.exe" -register:user -targetargs:"/nologo src\FluentAspect.Weaver.Tests\bin\Debug\FluentAspect.Weaver.Tests.dll" -output:coverage.xml
pause;
REM call lib\ReportGenerator_1.9.1.0\bin\ReportGenerator.exe coverage.xml ".\CodeCoverage\"