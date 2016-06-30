# ASP.NET Core (MVC 6) Tag Helpers for DevExtreme Widgets

[![Build status](https://ci.appveyor.com/api/projects/status/gyf4ghfeg5qjuxnl/branch/master?svg=true)](https://ci.appveyor.com/project/dxrobot/devextreme-aspnet-taghelpers/branch/master)
[![NuGet](https://img.shields.io/nuget/v/DevExtreme.AspNet.TagHelpers.svg)](https://www.nuget.org/packages/DevExtreme.AspNet.TagHelpers)
[![MIT License](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/DevExpress/DevExtreme.AspNet.TagHelpers/master/LICENSE)

## Synopsis

With Tag Helpers, ASP.NET Core Razor markup becomes more HTML-friendly, 
and Visual Studio code assistance is extended with IntelliSense for tags and their attributes.
[Read more...](https://docs.asp.net/en/latest/mvc/views/tag-helpers/intro.html)

DevExtreme ASP.NET Core Tag Helpers simplify the use of 
[UI and Visualizatioin widgets](http://js.devexpress.com/Demos/WidgetsGallery/) 
in Razor views
and connecting them to data exposed via MVC controllers.

```xml
<dx-data-grid>
    <datasource controller="ToDo" load-action="Items" />	
    <group-panel visible="true" />
    <filter-row visible="true" />
</dx-data-grid>
```

## Getting Started
 
 * [ASP.NET Core Prerequisites](https://docs.asp.net/en/latest/getting-started.html)
 * [Install Packages](https://github.com/DevExpress/DevExtreme.AspNet.TagHelpers/wiki/Install-Packages)
 * [Use DevExtreme Widgets](https://github.com/DevExpress/DevExtreme.AspNet.TagHelpers/wiki/Use-DevExtreme-Widgets)
 * [Specify Data Sources](https://github.com/DevExpress/DevExtreme.AspNet.TagHelpers/wiki/Specify-Data-Sources)
 * [Connect to Data Using Entity Framework Core](https://github.com/DevExpress/DevExtreme.AspNet.TagHelpers/wiki/Connect-to-Data-Using-Entity-Framework-Core)
 * [Use JavaScript with Tag Helpers (Event Handlers, Templates, etc)](https://github.com/DevExpress/DevExtreme.AspNet.TagHelpers/wiki/Use-JavaScript-with-Tag-Helpers-(Event-Handlers,-Templates,-etc))
 * [Samples](https://github.com/DevExpress/DevExtreme.AspNet.TagHelpers/tree/master/Samples)
  
## API Reference

DevExtreme ASP.NET Core Tag Helpers mirror 
[DevExtreme JavaScript API](http://js.devexpress.com/Documentation/ApiReference/).
  
## License

Familiarize yourself with the
[DevExtreme Commerical License](https://www.devexpress.com/Support/EULAs/DevExtreme.xml).  
[Free trial is available!](http://js.devexpress.com/Buy/) 

**DevExtreme ASP.NET Core Tag Helpers are released as a MIT-licensed (free and open-source) add-on to DevExtreme.**

## Support & Feedback

* For general ASP.NET Core, MVC 6 and Entity Framework Core topics, follow [these guidelines](https://github.com/aspnet/Home/blob/dev/CONTRIBUTING.md)
* For questions regarding DevExtreme libraries and JavaScript API, use [DevExpress Support Center](https://www.devexpress.com/Support/Center)
* For DevExtreme ASP.NET Core Tag Helpers bugs, questions and suggestions, use the [GitHub issue tracker](https://github.com/DevExpress/DevExtreme.AspNet.TagHelpers/issues)
