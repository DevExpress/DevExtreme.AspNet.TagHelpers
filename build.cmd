@echo off

where dotnet
if errorlevel 1 (
    echo "Error: .NET Core SDK is not available"
    exit /b 1
)

cd %~dp0

dotnet restore

pushd DevExtreme.AspNet.TagHelpers.Generator
dotnet run || (
    echo "Error: Generator failed, please report this bug"
    exit /b 1
)
popd

dotnet build DevExtreme.AspNet.TagHelpers
