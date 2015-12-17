using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevExtreme.AspNet.TagHelpers.Generator {

    static class CollectionItemsRegistry {
        static ICollection<string> _knownCollectionItemFullKeys = new HashSet<string> {
            "DevExtreme.AspNet.TagHelpers.dxDataGrid.Columns",
            "DevExtreme.AspNet.TagHelpers.dxDataGrid.SortByGroupSummaryInfo",
            "DevExtreme.AspNet.TagHelpers.dxDataGrid.Summary.GroupItems",
            "DevExtreme.AspNet.TagHelpers.dxDataGrid.Summary.TotalItems",
            "DevExtreme.AspNet.TagHelpers.dxChart.ArgumentAxis.ConstantLines",
            "DevExtreme.AspNet.TagHelpers.dxChart.ArgumentAxis.Strips",
            "DevExtreme.AspNet.TagHelpers.dxChart.Panes",
            "DevExtreme.AspNet.TagHelpers.dxChart.Series",
            "DevExtreme.AspNet.TagHelpers.dxChart.ValueAxis",
            "DevExtreme.AspNet.TagHelpers.dxChart.ValueAxis.ConstantLines",
            "DevExtreme.AspNet.TagHelpers.dxChart.ValueAxis.Strips",
            "DevExtreme.AspNet.TagHelpers.dxPieChart.Series",
            "DevExtreme.AspNet.TagHelpers.dxScheduler.Resources",
            "DevExtreme.AspNet.TagHelpers.dxRangeSelector.Chart.Series",
            "DevExtreme.AspNet.TagHelpers.Data.Datasource.Fields"
        };

        static IDictionary<string, string> _renamings = new Dictionary<string, string> {
            { "columns", "column"},
            { "groupItems", "groupItem" },
            { "totalItems", "totalItem" },
            { "constantLines", "constantLine" },
            { "strips", "strip" },
            { "panes", "pane" },
            { "resources", "resource"},
            { "fields", "field"}
        };

        public static string GetModifiedElementName(string name) {
            return _renamings.ContainsKey(name) ? _renamings[name] : name;
        }

        public static bool IsKnownCollectionItem(string fullKey) {
            return _knownCollectionItemFullKeys.Contains(fullKey);
        }

        public static bool SuspectCollectionItem(string rawType) {
            if(String.IsNullOrEmpty(rawType))
                return false;

            var parts = rawType.ToLower().Split('|');

            if(!parts.Contains("array"))
                return false;

            if(parts.Length == 1)
                return true;

            if(parts.Length < 3 && parts.Contains("object"))
                return true;

            return false;
        }
    }

}
