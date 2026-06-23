@echo off

dotnet publish -c Release

if %ERRORLEVEL% == 0 (
    xcopy "data" "Builds/net10.0/win-x64/publish/" /E /I /Y
) else (

)