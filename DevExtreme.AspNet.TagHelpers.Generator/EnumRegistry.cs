using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DevExtreme.AspNet.TagHelpers.Generator {

    static class EnumRegistry {
        static readonly ICollection<string> KnownNonEnums = new HashSet<string> {
            "DevExtreme.AspNet.TagHelpers.dxDataGrid.Pager.AllowedPageSizes",
            "DevExtreme.AspNet.TagHelpers.dxDataGrid.Pager.Visible",
            "DevExtreme.AspNet.TagHelpers.dxScheduler.CellDuration"
        };

        public static readonly IDictionary<string, string> InvertedKnownEnumns;

        static EnumRegistry() {
            InvertedKnownEnumns = new Dictionary<string, string>();

            foreach(var entry in KnownEnumns) {
                foreach(var fullKey in entry.Value.Props)
                    InvertedKnownEnumns.Add(fullKey, entry.Key);
            }
        }

        public static bool SuspectEnum(XElement element) {
            return element.Element("Values").Elements().Any();
        }

        public static void ValidateEnum(XElement element, string parentFullKey) {
            var fullName = parentFullKey + "." + Utils.ToCamelCase(element.GetName());

            if(KnownNonEnums.Contains(fullName))
                return;

            var intellisenseValues = element.Element("Values").Elements()
                .Select(i => i.GetName().Trim())
                .Where(i => i != "undefined")
                .Select(i => i.Trim('\'', '"'))
                .OrderBy(i => i)
                .ToArray();

            if(!InvertedKnownEnumns.ContainsKey(fullName)) {
                var members = intellisenseValues.Select(i => '"' + i + '"');
                var suggestedEnums = SuggestEnumNames(intellisenseValues);

                var msg = suggestedEnums.Any()
                    ? $"Suggested enums for \"{fullName}\": {String.Join(", ", suggestedEnums)}"
                    : $"Unknown enum: \"{fullName}\" {{ {String.Join(", ", members)} }}";

                throw new Exception(msg);
            }

            var knownValues = KnownEnumns[InvertedKnownEnumns[fullName]].SortedValues;

            var missingValues = intellisenseValues.Except(knownValues);
            if(missingValues.Any())
                throw new Exception($"Missing '{fullName}' enum values: {String.Join(",", missingValues)}.");

            var redundantValues = knownValues.Except(intellisenseValues);
            if(redundantValues.Any())
                throw new Exception($"Redundant '{fullName}' enum values: {String.Join(",", redundantValues)}");
        }

        static IReadOnlyList<string> SuggestEnumNames(string[] sortedValues) {
            return KnownEnumns.Where(e => e.Value.SortedValues.SequenceEqual(sortedValues)).Select(e => e.Key).ToArray();
        }

        public static readonly IDictionary<string, EnumInfo> KnownEnumns = new Dictionary<string, EnumInfo> {
            {
                "HorizontalAlignment",
                new EnumInfo(
                    new[] { "left", "center", "right" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxDataGrid.Column.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxDataGrid.Summary.TotalItem.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ArgumentAxis.ConstantLine.Label.HorizontalAlignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Legend.HorizontalAlignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Legend.ItemsAlignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Title.HorizontalAlignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ArgumentAxis.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ArgumentAxis.ConstantLineStyle.Label.HorizontalAlignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ArgumentAxis.Strip.Label.HorizontalAlignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ArgumentAxis.StripStyle.Label.HorizontalAlignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonAxisSettings.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonAxisSettings.StripStyle.Label.HorizontalAlignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Area.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Bar.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Bubble.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.CandleStick.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedArea.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedBar.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedLine.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedSpline.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedSplineArea.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Line.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.RangeArea.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.RangeBar.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Scatter.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Spline.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.SplineArea.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedArea.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedBar.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedLine.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedSpline.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedSplineArea.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StepArea.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StepLine.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Stock.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Series.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ValueAxis.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ValueAxis.ConstantLine.Label.HorizontalAlignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ValueAxis.ConstantLineStyle.Label.HorizontalAlignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ValueAxis.Strip.Label.HorizontalAlignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ValueAxis.StripStyle.Label.HorizontalAlignment",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.Legend.HorizontalAlignment",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.Legend.ItemsAlignment",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.Title.HorizontalAlignment",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Title.HorizontalAlignment",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Area.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Bar.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Bubble.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.CandleStick.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedArea.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedBar.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedLine.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedSpline.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedSplineArea.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Line.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.RangeArea.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.RangeBar.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Scatter.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Spline.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.SplineArea.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedArea.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedBar.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedLine.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedSpline.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedSplineArea.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StepArea.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StepLine.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Stock.Label.Alignment",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.Series.Label.Alignment",
                    }
                )
            },
            {
                "VerticalAlignment",
                new EnumInfo (
                    new[] { "bottom", "center", "top" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxChart.ArgumentAxis.ConstantLine.Label.VerticalAlignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ArgumentAxis.ConstantLineStyle.Label.VerticalAlignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ArgumentAxis.Strip.Label.VerticalAlignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ArgumentAxis.StripStyle.Label.VerticalAlignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonAxisSettings.StripStyle.Label.VerticalAlignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ValueAxis.ConstantLine.Label.VerticalAlignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ValueAxis.ConstantLineStyle.Label.VerticalAlignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ValueAxis.Strip.Label.VerticalAlignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ValueAxis.StripStyle.Label.VerticalAlignment"
                    }
                )
            },
            {
                "VerticalEdgeAlignment",
                new EnumInfo(
                    new[] { "bottom", "top" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxChart.Legend.VerticalAlignment",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Title.VerticalAlignment",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.Legend.VerticalAlignment",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.Title.VerticalAlignment",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Title.VerticalAlignment"
                    }
                )
            },
            {
                "GridColumnDataType",
                new EnumInfo(
                    new[] { "string", "number", "date", "boolean", "object" },
                    new[] { "DevExtreme.AspNet.TagHelpers.dxDataGrid.Column.DataType" }
                )
            },
            {
                "FilterOperations",
                new EnumInfo(
                    new[] { "=", "<>", "<", "<=", ">", ">=", "notcontains", "contains", "startswith", "endswith", "between" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxDataGrid.Column.FilterOperations",
                        "DevExtreme.AspNet.TagHelpers.dxDataGrid.Column.SelectedFilterOperation"
                    },
                    new Dictionary<string, string> {
                        { "=", "Equals" },
                        { ">", "Greater" },
                        { "<", "Less" },
                        { ">=", "GreaterOrEqual" },
                        { "<=", "LessOrEqual" },
                        { "<>", "NotEquals" },
                        { "notcontains", "NotContains" },
                        { "contains", "Contains" },
                        { "startswith", "StartsWith" },
                        { "endswith", "EndsWith" }
                    }
                )
            },
            {
                "FilterType",
                new EnumInfo(
                    new[] { "include", "exclude" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxDataGrid.Column.FilterType",
                        "DevExtreme.AspNet.TagHelpers.Data.Datasource.Field.FilterType"
                    }
                )
            },
            {
                "HeaderFilterGroupInterval",
                new EnumInfo(
                    new[] { "day", "dayOfWeek", "hour", "minute", "month", "quarter", "second", "year" },
                    new[] { "DevExtreme.AspNet.TagHelpers.dxDataGrid.Column.HeaderFilter.GroupInterval" }
                )
            },
            {
                "FixedColumnPosition",
                new EnumInfo(
                    new[] { "left", "right" },
                    new[] { "DevExtreme.AspNet.TagHelpers.dxDataGrid.Column.FixedPosition" }
                )
            },
            {
                "Format",
                new EnumInfo(
                    new[] {
                        "billions", "currency", "day", "decimal", "exponential", "fixedPoint", "largeNumber",
                        "longDate", "longTime", "millions", "millisecond", "month", "monthAndDay", "monthAndYear",
                        "percent", "quarter", "quarterAndYear", "shortDate", "shortTime", "thousands", "trillions", "year"
                    },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxDataGrid.Column.Format",
                        "DevExtreme.AspNet.TagHelpers.dxDataGrid.Summary.GroupItem.ValueFormat",
                        "DevExtreme.AspNet.TagHelpers.dxDataGrid.Summary.TotalItem.ValueFormat",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Tooltip.ArgumentFormat",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Tooltip.Format",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ArgumentAxis.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Area.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Bar.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Bubble.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.CandleStick.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedArea.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedBar.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedLine.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedSpline.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedSplineArea.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Line.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.RangeArea.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.RangeBar.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Scatter.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Spline.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.SplineArea.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedArea.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedBar.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedLine.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedSpline.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedSplineArea.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StepArea.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StepLine.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Stock.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Series.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ValueAxis.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.Tooltip.ArgumentFormat",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.Tooltip.Format",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.Series.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.CommonSeriesSettings.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.SliderMarker.Format",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Scale.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Area.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Bar.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Bubble.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.CandleStick.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedArea.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedBar.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedLine.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedSpline.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedSplineArea.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Line.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.RangeArea.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.RangeBar.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Scatter.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Spline.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.SplineArea.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedArea.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedBar.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedLine.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedSpline.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedSplineArea.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StepArea.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StepLine.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Stock.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.Series.Label.Format",
                        "DevExtreme.AspNet.TagHelpers.Data.Datasource.Field.Format"
                    }
                )
            },
            {
                "SparklineTooltipFormat",
                new EnumInfo(
                    new[] {
                        "billions", "currency", "decimal", "exponential", "fixedPoint", "largeNumber", "millions",
                        "thousands", "trillions" },
                    new[] { "DevExtreme.AspNet.TagHelpers.dxSparkline.Tooltip.Format" }
                )
            },
            {
                "SortOrder",
                new EnumInfo(
                    new[] { "asc", "desc" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxDataGrid.Column.SortOrder",
                        "DevExtreme.AspNet.TagHelpers.dxDataGrid.SortByGroupSummaryInfo.SortOrder",
                        "DevExtreme.AspNet.TagHelpers.Data.Datasource.Field.SortOrder"
                    }
                )
            },
            {
                "GridEditMode",
                new EnumInfo(
                    new[] { "batch", "cell", "row", "form" },
                    new[] { "DevExtreme.AspNet.TagHelpers.dxDataGrid.Editing.Mode" }
                )
            },
            {
                "GridFilterRowApplyMode",
                new EnumInfo(
                    new[] { "auto", "onClick" },
                    new[] { "DevExtreme.AspNet.TagHelpers.dxDataGrid.FilterRow.ApplyFilter" }
                )
            },
            {
                "GridScrollingMode",
                new EnumInfo(
                    new[] { "infinite", "standard", "virtual" },
                    new[] { "DevExtreme.AspNet.TagHelpers.dxDataGrid.Scrolling.Mode" }
                )
            },
            {
                "GridScrollbarDisplayMode",
                new EnumInfo(
                    new[] { "always", "never", "onHover", "onScroll" },
                    new[] { "DevExtreme.AspNet.TagHelpers.dxDataGrid.Scrolling.ShowScrollbar" }
                )
            },
            {
                "GridSelectionMode",
                new EnumInfo(
                    new[] { "multiple", "none", "single" },
                    new[] { "DevExtreme.AspNet.TagHelpers.dxDataGrid.Selection.Mode" }
                )
            },
            {
                "GridSelectionShowCheckBoxesMode",
                new EnumInfo(
                    new[] { "always", "none", "onClick", "onLongTap" },
                    new[] { "DevExtreme.AspNet.TagHelpers.dxDataGrid.Selection.ShowCheckBoxesMode" }
                )
            },
            {
                "GridSortingMode",
                new EnumInfo(
                    new[]  { "multiple", "none", "single" },
                    new[] { "DevExtreme.AspNet.TagHelpers.dxDataGrid.Sorting.Mode" }
                )
            },
                {
                "StateStoringType",
                new EnumInfo(
                    new[] { "custom", "localStorage", "sessionStorage" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxDataGrid.StateStoring.Type",
                        "DevExtreme.AspNet.TagHelpers.dxPivotGrid.StateStoring.Type"
                    }
                )
            },
            {
                "SummaryType",
                new EnumInfo(
                    new[]  { "avg", "count", "custom", "max", "min", "sum" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxDataGrid.Summary.GroupItem.SummaryType",
                        "DevExtreme.AspNet.TagHelpers.dxDataGrid.Summary.TotalItem.SummaryType",
                        "DevExtreme.AspNet.TagHelpers.Data.Datasource.Field.SummaryType"
                    }
                )
            },
            {
                "VizTheme",
                new EnumInfo (
                    new[] { "android5.light", "generic.dark", "generic.light", "ios7.default", "win8.black", "win8.white", "generic.contrast" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxChart.Theme",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.Theme",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Theme",
                        "DevExtreme.AspNet.TagHelpers.dxSparkline.Theme"
                    },
                    new Dictionary<string,string> {
                        ["ios7.default"] = "iOS7Default"
                    }
                )
            },
            {
                "VizPalette",
                new EnumInfo (
                    new[] { "Bright", "Default", "Harmony Light", "Ocean", "Pastel", "Soft", "Soft Pastel", "Vintage", "Violet" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxChart.Palette",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.Palette",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.Palette"
                    }
                )
            },
            {
                "ChartElementSelectionMode",
                new EnumInfo (
                    new[] { "multiple", "single" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxChart.PointSelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.SeriesSelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.PointSelectionMode"
                    }
                )
            },
            {
                "ChartOverlappingResolution",
                new EnumInfo (
                    new[] { "hide", "none", "stack" },
                    new[] { "DevExtreme.AspNet.TagHelpers.dxChart.ResolveLabelOverlapping" }
                )
            },
            {
                "PieChartOverlappingResolution",
                new EnumInfo (
                    new[] { "hide", "none", "shift" },
                    new [] { "DevExtreme.AspNet.TagHelpers.dxPieChart.ResolveLabelOverlapping" }
                )
            },
            {
                "OverlappingBehaviorMode",
                new EnumInfo (
                    new[]  { "enlargeTickInterval", "ignore", "rotate", "stagger" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxChart.ArgumentAxis.Label.OverlappingBehavior.Mode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonAxisSettings.Label.OverlappingBehavior.Mode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ValueAxis.Label.OverlappingBehavior.Mode"
                    }
                )
            },
            {
                "ChartManipulationMode",
                new EnumInfo (
                    new[] { "all", "mouse", "none", "touch" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxChart.ScrollingMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ZoomingMode"
                    }
                )
            },
            {
                "ChartAnimationEasing",
                new EnumInfo (
                    new[] { "easeOutCubic", "linear" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxChart.Animation.Easing",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.Animation.Easing"
                    }
                )
            },
            {
                "Position",
                new EnumInfo (
                    new[] { "bottom", "left", "right", "top" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxChart.Legend.ItemTextPosition",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ArgumentAxis.Position",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ScrollBar.Position",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ValueAxis.Position",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.Legend.ItemTextPosition"
                    }
                )
            },
            {
                "LegendOrientation",
                new EnumInfo (
                    new[] { "horizontal", "vertical" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxChart.Legend.Orientation",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.Legend.Orientation"
                    }
                )
            },
            {
                "ChartLegendHoverMode",
                new EnumInfo (
                    new[] { "excludePoints", "includePoints", "none" },
                    new[] { "DevExtreme.AspNet.TagHelpers.dxChart.Legend.HoverMode" }
                )
            },
            {
                "PieChartLegendHoverMode",
                new EnumInfo(
                    new[] { "none", "allArgumentPoints" },
                    new[] { "DevExtreme.AspNet.TagHelpers.dxPieChart.Legend.HoverMode" }
                )
            },
            {
                "RelativePosition",
                new EnumInfo (
                    new[] { "inside", "outside" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxChart.Legend.Position",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ArgumentAxis.ConstantLineStyle.Label.Position",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonAxisSettings.ConstantLineStyle.Label.Position",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Label.Position",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Bar.Label.Position",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Bubble.Label.Position",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedBar.Label.Position",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.RangeArea.Label.Position",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.RangeBar.Label.Position",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedBar.Label.Position",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Series.Label.Position",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ValueAxis.ConstantLineStyle.Label.Position",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Label.Position",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Bar.Label.Position",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Bubble.Label.Position",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedBar.Label.Position",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.RangeArea.Label.Position",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.RangeBar.Label.Position",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedBar.Label.Position",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.Series.Label.Position",
                    }
                )
            },
            {
                "ChartTooltipLocation",
                new EnumInfo (
                    new[] { "center", "edge" },
                    new[] { "DevExtreme.AspNet.TagHelpers.dxChart.Tooltip.Location" }
                )
            },
            {
                "DiscreteAxisDivisionMode",
                new EnumInfo (
                    new[] { "betweenLabels", "crossLabels" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxChart.ArgumentAxis.DiscreteAxisDivisionMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonAxisSettings.DiscreteAxisDivisionMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ValueAxis.DiscreteAxisDivisionMode"
                    }
                )
            },
            {
                "ChartDataType",
                new EnumInfo (
                    new[] { "datetime", "numeric", "string" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxChart.ArgumentAxis.ArgumentType",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ValueAxis.ValueType",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.CommonSeriesSettings.ArgumentType",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.Series.ArgumentType",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Scale.ValueType",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.ValueAxis.ValueType"
                    },
                    new Dictionary<string,string> {
                        ["datetime"] = "DateTime"
                    }
                )
            },
            {
                "ArgumentAxisHoverMode",
                new EnumInfo (
                    new[] { "allArgumentPoints", "none" } ,
                    new[] { "DevExtreme.AspNet.TagHelpers.dxChart.ArgumentAxis.HoverMode" }
                )
            },
            {
                "ChartPointInteractionMode",
                new EnumInfo (
                    new[] { "allArgumentPoints", "allSeriesPoints", "none", "onlyPoint" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Area.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Area.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedArea.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedArea.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedLine.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedLine.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedSpline.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedSpline.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedSplineArea.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Line.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Line.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.RangeArea.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.RangeArea.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Scatter.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Scatter.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Spline.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Spline.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.SplineArea.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.SplineArea.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedArea.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedArea.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedLine.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedLine.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedSpline.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedSpline.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedSplineArea.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StepArea.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StepArea.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StepLine.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StepLine.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Series.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Series.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Area.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Area.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedArea.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedArea.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedLine.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedLine.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedSpline.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedSpline.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedSplineArea.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Line.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Line.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.RangeArea.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.RangeArea.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Scatter.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Scatter.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Spline.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Spline.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.SplineArea.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.SplineArea.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedArea.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedArea.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedLine.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedLine.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedSpline.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedSpline.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedSplineArea.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StepArea.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StepArea.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StepLine.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StepLine.Point.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.Series.Point.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.Series.Point.SelectionMode"
                    }
                )
            },
            {
                "AxisScale",
                new EnumInfo (
                    new[] { "continuous", "discrete", "logarithmic" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxChart.ArgumentAxis.Type",
                        "DevExtreme.AspNet.TagHelpers.dxChart.ValueAxis.Type",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Scale.Type"
                    }
                )
            },
            {
                "DashStyle",
                new EnumInfo (
                    new[] { "dash", "dot", "longDash", "solid" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Crosshair.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Series.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonPaneSettings.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.HoverStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.SelectionStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedLine.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedSpline.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Line.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Spline.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedLine.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedSpline.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StepLine.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Area.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Area.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Area.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Area.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Bar.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Bar.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Bar.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Bar.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Bubble.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Bubble.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Bubble.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Bubble.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.CandleStick.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedArea.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedArea.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedArea.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedArea.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedBar.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedBar.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedBar.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedBar.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedLine.HoverStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedLine.SelectionStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedLine.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedSpline.HoverStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedSpline.SelectionStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedSpline.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedSplineArea.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedSplineArea.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedSplineArea.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedSplineArea.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Line.HoverStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Line.SelectionStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Line.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.RangeArea.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.RangeArea.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.RangeArea.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.RangeArea.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.RangeBar.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.RangeBar.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.RangeBar.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.RangeBar.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Scatter.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Spline.HoverStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Spline.SelectionStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Spline.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.SplineArea.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.SplineArea.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.SplineArea.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.SplineArea.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedArea.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedArea.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedArea.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedArea.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedBar.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedBar.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedBar.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedBar.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedLine.HoverStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedLine.SelectionStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedLine.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedSpline.HoverStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedSpline.SelectionStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedSpline.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedSplineArea.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedSplineArea.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedSplineArea.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StepArea.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StepArea.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StepArea.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StepArea.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StepLine.HoverStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StepLine.SelectionStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StepLine.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Stock.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Crosshair.HorizontalLine.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Crosshair.VerticalLine.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Legend.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Pane.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Series.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Series.HoverStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Series.SelectionStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Series.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Series.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Series.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Tooltip.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.Legend.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.Series.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.Series.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.Series.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.Series.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.Tooltip.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.CommonSeriesSettings.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.CommonSeriesSettings.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.CommonSeriesSettings.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.CommonSeriesSettings.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.HoverStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.SelectionStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedLine.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedSpline.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Line.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Spline.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedLine.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedSpline.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StepLine.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Area.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Area.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Area.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Area.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Bar.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Bar.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Bar.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Bar.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Bubble.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Bubble.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Bubble.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Bubble.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.CandleStick.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedArea.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedArea.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedArea.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedArea.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedBar.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedBar.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedBar.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedBar.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedLine.HoverStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedLine.SelectionStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedLine.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedSpline.HoverStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedSpline.SelectionStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedSpline.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedSplineArea.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedSplineArea.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedSplineArea.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedSplineArea.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Line.HoverStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Line.SelectionStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Line.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.RangeArea.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.RangeArea.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.RangeArea.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.RangeArea.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.RangeBar.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.RangeBar.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.RangeBar.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.RangeBar.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Scatter.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Spline.HoverStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Spline.SelectionStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Spline.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.SplineArea.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.SplineArea.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.SplineArea.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.SplineArea.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedArea.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedArea.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedArea.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedArea.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedBar.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedBar.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedBar.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedBar.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedLine.HoverStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedLine.SelectionStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedLine.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedSpline.HoverStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedSpline.SelectionStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedSpline.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedSplineArea.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedSplineArea.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedSplineArea.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StepArea.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StepArea.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StepArea.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StepArea.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StepLine.HoverStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StepLine.SelectionStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StepLine.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Stock.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.Series.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.Series.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.Series.HoverStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.Series.SelectionStyle.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.Series.HoverStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.Series.Label.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.Series.SelectionStyle.Border.DashStyle",
                        "DevExtreme.AspNet.TagHelpers.dxSparkline.Tooltip.Border.DashStyle"
                    }
                )
            },
            {
                "SeriesType",
                new EnumInfo (
                    new[] {
                        "area", "bar", "bubble", "candlestick", "fullstackedarea", "fullstackedbar", "fullstackedline", "fullstackedspline",
                        "fullstackedsplinearea", "line", "rangearea", "rangebar", "scatter", "spline", "splinearea", "stackedarea", "stackedbar",
                        "stackedline", "stackedspline", "stackedsplinearea", "steparea", "stepline", "stock" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxChart.Series.Type",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Type",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Type",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.Series.Type"
                    },
                    new Dictionary<string, string> {
                        ["fullstackedarea"] = "FullStackedArea",
                        ["fullstackedbar"] = "FullStackedBar",
                        ["fullstackedline"] = "FullStackedLine",
                        ["fullstackedspline"] = "FullStackedSpline",
                        ["fullstackedsplinearea"] = "FullStackedSplineArea",
                        ["rangearea"] = "RangeArea",
                        ["rangebar"] = "RangeBar",
                        ["splinearea"] = "SplineArea",
                        ["stackedarea"] = "StackedArea",
                        ["stackedbar"] = "StackedBar",
                        ["stackedline"] = "StackedLine",
                        ["stackedspline"] = "StackedSpline",
                        ["stackedsplinearea"] = "StackedSplineArea",
                        ["steparea"] = "StepArea",
                        ["stepline"] = "StepLine"
                    }
                )
            },
            {
                "SparklineType",
                new EnumInfo(
                    new[] { "area", "bar", "line", "spline", "splinearea", "steparea", "stepline", "winloss" },
                    new[] { "DevExtreme.AspNet.TagHelpers.dxSparkline.Type" },
                    new Dictionary<string, string> {
                        ["splinearea"] = "SplineArea",
                        ["steparea"] = "StepArea",
                        ["stepline"] = "StepLine",
                        ["winloss"] = "WinLoss"
                    }
                )
            },
            {
                "PointSymbol",
                new EnumInfo (
                    new[] { "circle", "cross", "polygon", "square", "triangleDown", "triangleUp" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Area.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedArea.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedLine.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedSpline.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedSplineArea.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Line.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.RangeArea.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Scatter.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Spline.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.SplineArea.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedArea.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedLine.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedSpline.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedSplineArea.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StepArea.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StepLine.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Series.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Area.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedArea.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedLine.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedSpline.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedSplineArea.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Line.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.RangeArea.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Scatter.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Spline.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.SplineArea.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedArea.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedLine.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedSpline.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedSplineArea.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StepArea.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StepLine.Point.Symbol",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.Series.Point.Symbol",
                    }
                )
            },
            {
                "SparklinePointSymbol",
                new EnumInfo(
                    new[] { "circle", "cross", "polygon", "square", "triangle" },
                    new[] { "DevExtreme.AspNet.TagHelpers.dxSparkline.PointSymbol" }
                )
            },
            {
                "FinancialChartReductionLevel",
                new EnumInfo (
                    new[] { "close", "high", "low", "open" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Reduction.Level",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.CandleStick.Reduction.Level",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Stock.Reduction.Level",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Series.Reduction.Level",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Reduction.Level",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.CandleStick.Reduction.Level",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Stock.Reduction.Level",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.Series.Reduction.Level",
                    }
                )
            },
            {
                "ValueErrorBarDisplayMode",
                new EnumInfo (
                    new[] { "auto", "high", "low", "none" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.ValueErrorBar.DisplayMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Area.ValueErrorBar.DisplayMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Bar.ValueErrorBar.DisplayMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Line.ValueErrorBar.DisplayMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Scatter.ValueErrorBar.DisplayMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.SplineArea.ValueErrorBar.DisplayMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StepArea.ValueErrorBar.DisplayMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StepLine.ValueErrorBar.DisplayMode",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Series.ValueErrorBar.DisplayMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.ValueErrorBar.DisplayMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Area.ValueErrorBar.DisplayMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Bar.ValueErrorBar.DisplayMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Line.ValueErrorBar.DisplayMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Scatter.ValueErrorBar.DisplayMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.SplineArea.ValueErrorBar.DisplayMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StepArea.ValueErrorBar.DisplayMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StepLine.ValueErrorBar.DisplayMode",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.Series.ValueErrorBar.DisplayMode",
                    }
                )
            },
            {
                "ValueErrorBarType",
                new EnumInfo (
                    new[] { "fixed", "percent", "stdDeviation", "stdError", "variance" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.ValueErrorBar.Type",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Area.ValueErrorBar.Type",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Bar.ValueErrorBar.Type",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Line.ValueErrorBar.Type",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Scatter.ValueErrorBar.Type",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.SplineArea.ValueErrorBar.Type",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StepArea.ValueErrorBar.Type",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StepLine.ValueErrorBar.Type",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Series.ValueErrorBar.Type",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.ValueErrorBar.Type",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Area.ValueErrorBar.Type",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Bar.ValueErrorBar.Type",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Line.ValueErrorBar.Type",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Scatter.ValueErrorBar.Type",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.SplineArea.ValueErrorBar.Type",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StepArea.ValueErrorBar.Type",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StepLine.ValueErrorBar.Type",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.Series.ValueErrorBar.Type",
                    }
                )
            },
            {
                "HatchingDirection",
                new EnumInfo (
                    new[] { "left", "none", "right" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Area.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Area.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Bar.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Bar.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Bubble.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.Bubble.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.CandleStick.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.CandleStick.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedArea.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedArea.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedBar.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedBar.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedSplineArea.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.FullStackedSplineArea.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.RangeArea.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.RangeArea.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.RangeBar.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.RangeBar.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.SplineArea.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.SplineArea.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedArea.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedArea.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedBar.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedBar.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedSplineArea.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StackedSplineArea.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StepArea.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.CommonSeriesSettings.StepArea.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Series.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxChart.Series.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.Series.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.Series.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.CommonSeriesSettings.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.CommonSeriesSettings.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Area.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Area.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Bar.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Bar.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Bubble.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.Bubble.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.CandleStick.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.CandleStick.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedArea.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedArea.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedBar.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedBar.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedSplineArea.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.FullStackedSplineArea.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.RangeArea.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.RangeArea.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.RangeBar.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.RangeBar.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.SplineArea.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.SplineArea.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedArea.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedArea.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedBar.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedBar.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedSplineArea.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StackedSplineArea.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StepArea.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.CommonSeriesSettings.StepArea.SelectionStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.Series.HoverStyle.Hatching.Direction",
                        "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.Series.SelectionStyle.Hatching.Direction"
                    }
                )
            },
            {
                "SchedulerView",
                new EnumInfo (
                    new[] { "day", "month", "week", "workWeek", "timelineDay", "timelineWeek", "timelineWorkWeek" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxScheduler.CurrentView",
                        "DevExtreme.AspNet.TagHelpers.dxScheduler.Views"
                    }
                )
            },
            {
                "SchedulerRecurrenceEditMode",
                new EnumInfo(
                    new[] { "dialog", "occurrence", "series" },
                    new[] { "DevExtreme.AspNet.TagHelpers.dxScheduler.RecurrenceEditMode" }
                )
            },
            {
                "FirstDayOfWeek",
                new EnumInfo(
                    new[] { "0", "1", "2", "3", "4", "5", "6" },
                    new[] { "DevExtreme.AspNet.TagHelpers.dxScheduler.FirstDayOfWeek" },
                    new Dictionary<string, string> {
                        ["0"] = "Sunday",
                        ["1"] = "Monday",
                        ["2"] = "Tuesday",
                        ["3"] = "Wednesday",
                        ["4"] = "Thursday",
                        ["5"] = "Friday",
                        ["6"] = "Saturday"
                    }
                )
            },
            {
                "PieChartSeriesInteractionMode",
                new EnumInfo(
                    new[] { "none", "onlyPoint" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.Series.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.Series.SelectionMode",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.CommonSeriesSettings.HoverMode",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.CommonSeriesSettings.SelectionMode"
                    }
                )
            },
            {
                "PieChartSegmentsDirection",
                new EnumInfo (
                    new[] { "anticlockwise", "clockwise" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.Series.SegmentsDirection",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.SegmentsDirection"
                    },
                    new Dictionary<string, string> {
                        ["anticlockwise"] = "AntiClockwise"
                    }
                )
            },
            {
                "PieChartType",
                new EnumInfo(
                    new[] { "donut", "doughnut", "pie" },
                    new[] { "DevExtreme.AspNet.TagHelpers.dxPieChart.Type" }
                )
            },
            {
                "PieChartLabelPosition",
                new EnumInfo(
                    new[] { "columns", "inside", "outside" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.Series.Label.Position",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.CommonSeriesSettings.Label.Position"
                    }
                )
            },
            {
                "SmallValuesGroupingMode",
                new EnumInfo(
                    new[] { "none", "smallValueThreshold", "topN" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.Series.SmallValuesGrouping.Mode",
                        "DevExtreme.AspNet.TagHelpers.dxPieChart.CommonSeriesSettings.SmallValuesGrouping.Mode"
                    }
                )
            },
            {
                "SelectedRangeChangedCallMode",
                new EnumInfo(
                    new[] { "onMoving", "onMovingComplete" },
                    new[] { "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Behavior.CallSelectedRangeChanged" }
                )
            },
            {
                "BackgroundImageLocation",
                new EnumInfo(
                    new[] { "center", "centerBottom", "centerTop", "full", "leftBottom", "leftCenter", "leftTop", "rightBottom", "rightCenter", "rightTop" },
                    new[] { "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Background.Image.Location" }
                )
            },
            {
                "RangeSelectorChartAxisScale",
                new EnumInfo(
                    new[] { "continuous", "logarithmic" },
                    new[] { "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.ValueAxis.Type" }
                )
            },
            {
                "RangeSelectorLabelFormat",
                new EnumInfo(
                    new[] {
                        "day", "longDate", "longTime", "millisecond", "month", "monthAndDay", "monthAndYear", "q", "Q",
                        "qq", "QQ", "quarter", "quarterAndYear", "shortDate", "shortTime", "year"
                    },
                    new[] { "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Scale.Marker.Label.Format" },
                    new Dictionary<string, string> {
                        { "q", "RomanQuarter" },
                        { "qq", "RomanQuarterWithPrefix" },
                        { "Q", "ArabicQuarter" },
                        { "QQ", "ArabicQuarterWithPrefix" }
                    }
                )
            },
            {
                "PivotGridFieldChooserLayout",
                new EnumInfo(
                    new[] { "0", "1", "2" },
                    new[] {
                        "DevExtreme.AspNet.TagHelpers.dxPivotGrid.FieldChooser.Layout",
                        "DevExtreme.AspNet.TagHelpers.dxPivotGridFieldChooser.Layout"
                    },
                    new Dictionary<string, string> {
                        { "0", "Layout0" },
                        { "1", "Layout1" },
                        { "2", "Layout2" }
                    }
                )
            },
            {
                "PivotGridTotalsDisplayMode",
                new EnumInfo(
                    new[] { "both", "columns", "none", "rows" },
                    new[] { "DevExtreme.AspNet.TagHelpers.dxPivotGrid.ShowTotalsPrior" }
                )
            },
            {
                "PivotGridScrollingMode",
                new EnumInfo(
                    new[] { "standard", "virtual" },
                    new[] { "DevExtreme.AspNet.TagHelpers.dxPivotGrid.Scrolling.Mode" }
                )
            },
            {
                "PivotGridArea",
                new EnumInfo(
                    new[] { "row", "column", "data" },
                    new[] { "DevExtreme.AspNet.TagHelpers.Data.Datasource.Field.Area" }
                )
            },
            {
                "PivotGridDataType",
                new EnumInfo(
                    new[] { "string", "number", "date" },
                    new[] { "DevExtreme.AspNet.TagHelpers.Data.Datasource.Field.DataType" }
                )
            },
            {
                "PivotGridGroupInterval",
                new EnumInfo(
                    new[] { "year", "quarter", "month", "day", "dayOfWeek" },
                    new[] { "DevExtreme.AspNet.TagHelpers.Data.Datasource.Field.GroupInterval" }
                )
            },
            {
                "PivotGridSortMode",
                new EnumInfo(
                    new[] { "caption", "value" },
                    new[] { "DevExtreme.AspNet.TagHelpers.Data.Datasource.Field.SortBy" }
                )
            },
            {
                "PivotGridRunningTotalMode",
                new EnumInfo(
                    new[] { "column", "row" },
                    new[] { "DevExtreme.AspNet.TagHelpers.Data.Datasource.Field.RunningTotal" }
                )
            },
            {
                "PivotGridSummaryDisplayMode",
                new EnumInfo(
                    new[] {
                        "absoluteVariation", "percentOfColumnGrandTotal", "percentOfColumnTotal", "percentOfGrandTotal",
                        "percentOfRowGrandTotal", "percentOfRowTotal", "percentVariation"
                    },
                    new[] { "DevExtreme.AspNet.TagHelpers.Data.Datasource.Field.SummaryDisplayMode" }
                )
            }
        };
    }

}
