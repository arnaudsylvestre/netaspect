call "%VS100COMNTOOLS%vsvars32.bat"
call msbuild %~dp0..\src\NetAspect.sln /target:rebuild /property:Configuration=Debug /verbosity:minimal
RMDIR Temp /S /Q
RMDIR Samples /S /Q
mkdir Samples
mkdir Temp\Nuget\Tools
mkdir Temp\Nuget\Content\Aspects
call ..\src\NetAspect.SamplesExtractor\bin\Debug\NetAspect.SamplesExtractor.exe ..\src\NetAspect.Weaver.Tests\Samples Samples

mkdir Temp\WithoutNuget
call xcopy %~dp0Nuget\* Temp\Nuget\Tools /D /E /C /R /H /I /K /Y
call xcopy %~dp0..\src\NugetItems\Tools\PEVerify.exe Temp /Y
call xcopy %~dp0..\src\NetAspect.Weaver.Tasks\bin\Debug Temp /D /E /C /R /H /I /K /Y
call xcopy %~dp0Samples\* Temp\Nuget\Tools /D /E /C /R /H /I /K /Y
