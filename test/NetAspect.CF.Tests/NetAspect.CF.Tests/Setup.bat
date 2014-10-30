call xcopy %~dp0..\..\..\src\NugetItems\Tools\NetAspect.Targets . /Y
call xcopy %~dp0..\..\..\src\NugetItems\Tools\PEVerify.exe . /Y
call xcopy %~dp0..\..\..\src\NetAspect.Weaver.Tasks\bin\Debug . /D /E /C /R /H /I /K /Y