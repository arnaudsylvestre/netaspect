call "%VS90COMNTOOLS%vsvars32.bat"
call msbuild %~dp0NetAspect.CF.Tests.csproj
