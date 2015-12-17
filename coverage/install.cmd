if not exist nuget.exe (PowerShell -Command wget https://dist.nuget.org/win-x86-commandline/latest/nuget.exe -OutFile nuget.exe)
nuget install ReportGenerator -Version 2.3.2.0
nuget install OpenCover -Version 4.6.166
