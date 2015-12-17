using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DevExtreme.AspNet.TagHelpers.Generator {

    class TagInfoPreProcessor {
        XElement _commonSeriesSettingsSample;
        XElement _seriesSample;

        public void Process(TagInfo tag) {
            ModifyWidget(tag);
            ModifyCollectionItem(tag);
            ModifyDatasourceOwner(tag);
            ModifyRangeSelectorChartOptions(tag);
            ModifyCommonSeriesSettings(tag);
            TurnChildrenIntoProps(tag);
            ValidateEnums(tag);
            ModifyPropTypes(tag);
        }

        static void ModifyWidget(TagInfo tag) {
            if(tag.ParentTagName != null)
                return;

            tag.ExtraChildRestrictions.Add("script");
            tag.BaseClassName = "WidgetTagHelper";
            TargetElementsRegistry.InnerScriptTargets.Add(tag.GetTagName());
        }

        static void ModifyCollectionItem(TagInfo tag) {
            if(!CollectionItemsRegistry.SuspectCollectionItem(tag.Element.Attribute("Type")?.Value))
                return;

            if(!CollectionItemsRegistry.IsKnownCollectionItem(tag.GetFullKey()))
                throw new Exception($"New collection suspect detected: \"{tag.GetFullKey()}\"");

            tag.Element.SetName(CollectionItemsRegistry.GetModifiedElementName(tag.Element.GetName()));
            tag.BaseClassName = "CollectionItemTagHelper";
        }

        static void ModifyDatasourceOwner(TagInfo tag) {
            var datasourceProp = tag.PropElements.FirstOrDefault(s => s.GetName() == "dataSource");
            if(datasourceProp == null)
                return;

            tag.PropElements.Remove(datasourceProp);
            tag.ExtraChildRestrictions.Add("datasource");

            TargetElementsRegistry.DatasourceTargets.Add(tag.GetTagName());
        }

        void ModifyRangeSelectorChartOptions(TagInfo tag) {
            if(tag.GetFullKey() == "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings") {
                _commonSeriesSettingsSample = new XElement(tag.Element);
                return;
            }

            if(tag.GetFullKey() == "DevExtreme.AspNet.TagHelpers.dxChart.Series") {
                _seriesSample = new XElement(tag.Element);
                return;
            }

            if(tag.GetFullKey() == "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart") {
                ReplaceWithClone(tag, "commonSeriesSettings", _commonSeriesSettingsSample);
                ReplaceWithClone(tag, "series", _seriesSample);
            }
        }

        static void ReplaceWithClone(TagInfo tag, string name, XElement sample) {
            var element = tag.PropElements.FirstOrDefault(el => el.GetName() == name);
            var clone = new XElement(sample);
            element.ReplaceWith(clone);
            tag.PropElements.Remove(element);
            tag.ChildTagElements.Add(clone);
        }

        static void ModifyCommonSeriesSettings(TagInfo tag) {
            if(tag.GetFullKey() != "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings" &&
                tag.GetFullKey() != "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings")
                return;

            var seriesSettingsPrefix = "commonseriesoptions_";

            var propOfData = XDocument.Load("meta/PropOf.xml").Root.Elements()
                .Select(el => new {
                    DocID = el.Attribute("docid").Value,
                    PropertyOf = el.Attribute("propertyOf").Value
                })
                .Where(i => i.DocID.StartsWith(seriesSettingsPrefix))
                .ToDictionary(
                    i => i.DocID.Substring(seriesSettingsPrefix.Length),
                    i => new HashSet<string>(i.PropertyOf.Split(','), StringComparer.OrdinalIgnoreCase)
                );

            var seriesNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
                "area", "bar", "bubble", "candleStick", "fullStackedArea", "fullStackedBar", "fullStackedLine",
                "fullStackedSpline", "fullStackedSplineArea", "line", "rangeArea", "rangeBar", "scatter", "spline",
                "splineArea", "stackedArea", "stackedBar", "stackedLine", "stackedSpline", "stackedSplineArea",
                "stepArea", "stepLine", "stock"
            };

            foreach(var prop in tag.PropElements.Where(p => seriesNames.Contains(p.GetName())).ToArray()) {
                prop.Remove();
                tag.PropElements.Remove(prop);
            }

            var specificSeriesElement = new XElement(tag.Element);
            specificSeriesElement.GetPropElement("type").Remove();

            foreach(var name in seriesNames) {
                var element = new XElement(specificSeriesElement);
                element.SetAttributeValue("Name", name);

                foreach(var prop in propOfData.Where(p => !p.Value.Contains(name + "Series")))
                    RemoveNestedPropElement(prop.Key.Split('_'), element);

                tag.ChildTagElements.Add(element);
            }
        }

        static void RemoveNestedPropElement(IEnumerable<string> nameParts, XElement element) {
            foreach(var namePart in nameParts) {
                if(element == null)
                    break;

                element = element.GetPropElement(namePart);
            }

            element?.Remove();
        }

        static void TurnChildrenIntoProps(TagInfo tag) {
            var fullNames = new HashSet<string> {
                    "DevExtreme.AspNet.TagHelpers.dxChart.ArgumentAxis.TickInterval",
                    "DevExtreme.AspNet.TagHelpers.dxChart.ValueAxis.TickInterval",
                    "DevExtreme.AspNet.TagHelpers.dxChart.ArgumentAxis.MinorTickInterval",
                    "DevExtreme.AspNet.TagHelpers.dxChart.ValueAxis.MinorTickInterval",
                    "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Scale.MinorTickInterval",
                    "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Scale.TickInterval"
                };

            var migratingElements = tag.ChildTagElements
                .Where(el => fullNames.Contains(tag.GetFullKey() + "." + Utils.ToCamelCase(el.GetName())))
                .ToArray();

            foreach(var element in migratingElements) {
                element.Element("Values").RemoveAll();
                element.Element("Properties").Remove();
                tag.ChildTagElements.Remove(element);
                tag.PropElements.Add(element);
            }
        }

        static void ValidateEnums(TagInfo tag) {
            foreach(var propElement in tag.PropElements) {
                if(!EnumRegistry.SuspectEnum(propElement))
                    continue;

                EnumRegistry.ValidateEnum(propElement, tag.GetFullKey());
            }
        }

        static void ModifyPropTypes(TagInfo tag) {
            var enumsTable = EnumRegistry.InvertedKnownEnumns;
            var propsTable = PropTypeRegistry.OverrideTable;

            foreach(var propElement in tag.PropElements) {
                var fullName = tag.GetFullKey() + "." + Utils.ToCamelCase(propElement.GetName());
                var overridenType = String.Empty;

                if(enumsTable.ContainsKey(fullName)) {
                    if(propElement.GetRawType() == "array")
                        overridenType = $"IEnumerable<{enumsTable[fullName]}>";
                    else
                        overridenType = enumsTable[fullName];
                }

                if(propsTable.ContainsKey(fullName))
                    overridenType = propsTable[fullName];

                if(!String.IsNullOrEmpty(overridenType))
                    propElement.SetAttributeValue(PropTypeRegistry.CLR_TYPE_OVERRIDE_ATTR, overridenType);
            }
        }
    }

}
