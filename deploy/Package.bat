call "%VS100COMNTOOLS%vsvars32.bat"
call msbuild %~dp0..\src\NetAspect.sln /target:rebuild /property:Configuration=Debug /verbosity:minimal
RMDIR Temp /S /Q
RMDIR Output /S /Q
mkdir Output

REM Nuget
mkdir Temp\Nuget\Tools
mkdir Temp\Nuget\Content\Aspects
call ..\src\NetAspect.SamplesExtractor\bin\Debug\NetAspect.SamplesExtractor.exe ..\src\NetAspect.Weaver.Tests\Samples Temp\Nuget\Content\Aspects
call xcopy %~dp0Nuget\* Temp\Nuget\Tools /D /E /C /R /H /I /K /Y
call Nuget\Nuget.exe pack Nuget\NetAspect.nuspec -OutputDirectory Output

REM Without Nuget
mkdir Temp\WithoutNuget
call xcopy %~dp0..\src\NugetItems\Tools\PEVerify.exe Temp\WithoutNuget /Y
call xcopy %~dp0..\src\NetAspect.Weaver.Tasks\bin\Debug Temp\WithoutNuget /D /E /C /R /H /I /K /Y
call xcopy %~dp0Temp\Nuget\Content\Aspects\* Temp\WithoutNuget /D /E /C /R /H /I /K /Y
call Tools\fart.exe Temp\WithoutNuget\*.pp $rootNamespace$ NetAspect
call ren Temp\WithoutNuget\*.cs.pp *.cs
pushd .
cd Temp\WithoutNuget
call ..\..\Tools\7za.exe a ..\..\Output\NetAspect.zip *
popd

RMDIR Temp /S /Q