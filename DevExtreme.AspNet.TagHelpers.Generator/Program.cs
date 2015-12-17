using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DevExtreme.AspNet.TagHelpers.Generator {

    class Program {
        const string DX_VERSION = "15.2";

        static void Main(string[] args) {
            var widgetNames = new HashSet<string> {
                "dxChart",
                "dxDataGrid",
                "dxScheduler",
                "dxPieChart",
                "dxRangeSelector",
                "dxSparkline",
                "dxPivotGrid"
            };

            var ns = new[] { "DevExtreme.AspNet.TagHelpers" };
            var tagInfoPreProcessor = new TagInfoPreProcessor();
            var generator = new Generator(outputRoot: "../");
            generator.DeleteGeneratedFiles(ns);

            foreach(var obj in GetIntellisenseObjectsFor(widgetNames))
                generator.GenerateClass(new TagInfo(obj, tagInfoPreProcessor, ns, null));

            generator.GenerateEnums(ns, "Enums", EnumRegistry.KnownEnumns);

            generator.GenerateTargetElementsClass(
                ns,
                "InnerScriptTagHelper",
                TargetElementsRegistry.InnerScriptTargets.Select(CreateInnerScriptTarget)
            );

            generator.GenerateTargetElementsClass(
                ns.Concat("Data"),
                "LoadActionDatasourceTagHelper",
                TargetElementsRegistry.DatasourceTargets.Select(GetDataSourceTargetFactory("load-action"))
            );

            generator.GenerateTargetElementsClass(
                ns.Concat("Data"),
                "ItemsDatasourceTagHelper",
                TargetElementsRegistry.DatasourceTargets.Select(GetDataSourceTargetFactory("items"))
            );

            generator.GenerateTargetElementsClass(
                ns.Concat("Data"),
                "UrlDatasourceTagHelper",
                TargetElementsRegistry.DatasourceTargets.Select(GetDataSourceTargetFactory("url"))
            );

            generator.GenerateClass(
                CreatePivotGridDatasourceTag(tagInfoPreProcessor, ns),
                "PivotGridDatasourceTagHelper",
                isPartial: true,
                generateKeyProps: false
            );

            Console.WriteLine("Done");
        }

        static IEnumerable<XElement> GetIntellisenseObjectsFor(ICollection<string> names) {
            return XDocument.Load($"meta/IntellisenseData_{DX_VERSION}.xml")
                .Root
                .Elements("IntellisenseObjectInfo")
                .Where(o => names.Contains(o.GetName()));
        }

        static TargetElementInfo CreateInnerScriptTarget(string parentTagName) {
            return new TargetElementInfo {
                Tag = "script",
                ParentTag = parentTagName,
                IsSelfClosing = false
            };
        }

        static Func<string, TargetElementInfo> GetDataSourceTargetFactory(string bindingAttribute) {
            return parentTagName => new TargetElementInfo {
                Tag = "datasource",
                BindingAttribute = bindingAttribute,
                ParentTag = parentTagName,
                IsSelfClosing = parentTagName != "dx-pivot-grid"
            };
        }

        static TagInfo CreatePivotGridDatasourceTag(TagInfoPreProcessor tagInfoPreProcessor, IEnumerable<string> ns) {
            var xDoc = XDocument.Load($"meta/IntellisenseData_{DX_VERSION}_spec.xml");
            var element = new XElement(xDoc.Root.Elements("IntellisenseObjectInfo").Single(el => el.GetName() == "PivotGridDataSource"));
            element.SetName("datasource");
            element.GetPropElement("store").Remove();

            var result = new TagInfo(element, tagInfoPreProcessor, ns.Concat("Data"), parentTagName: "dx-pivot-grid");
            result.Key = "dataSource";
            result.BaseClassName = null;

            return result;
        }
    }

}
