where dnx > dnxpath.txt
set /p dnxpath=<dnxpath.txt

call dnu build ..\DevExtreme.AspNet.TagHelpers

opencover.4.6.166\tools\OpenCover.Console.exe ^
 "-target:%dnxpath%" ^
 "-targetargs: --lib %~dp0\..\DevExtreme.AspNet.TagHelpers\bin\Debug\net451 -p ..\DevExtreme.AspNet.TagHelpers.Tests xunit.runner.dnx" ^
 "-filter:+[DevExtreme*]*" ^
 "-excludebyattribute:DevExtreme.AspNet.TagHelpers.GeneratedAttribute" ^
 -hideskipped:All -output:coverage.xml -register:user

ReportGenerator.2.3.2.0\tools\ReportGenerator.exe -reports:coverage.xml -targetdir:report