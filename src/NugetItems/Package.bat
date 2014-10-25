del /Q Content\Aspects\*.*
call ..\NetAspect.SamplesExtractor\bin\Debug\NetAspect.SamplesExtractor.exe ..\NetAspect.Weaver.Tests\Samples Content\Aspects
NuGet.exe pack NetAspect.nuspec
xcopy *.nupkg C:\TempNuget /Y