using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DevExtreme.AspNet.TagHelpers.Generator {

    class EnumInfo {
        public readonly string[] SortedValues;
        public readonly string[] Props;
        public readonly IDictionary<string, string> Renamings;

        public EnumInfo(string[] values, string[] props, IDictionary<string, string> renamings = null) {
            SortedValues = values;
            Props = props;
            Renamings = renamings;
            Array.Sort(SortedValues);
        }

        public IEnumerable<RenderingItem> EnumerateForRendering() {
            return SortedValues.Select((value, index) => new RenderingItem {
                IsFirst = index == 0,
                JavaScriptValue = value,
                CSharpValue = RenameValueForCSharp(value)
            });
        }

        string RenameValueForCSharp(string value) {
            if(Renamings != null && Renamings.ContainsKey(value))
                return Renamings[value];

            return Utils.ToCamelCase(RemoveSpecialChars(value));
        }

        static string RemoveSpecialChars(string value) {
            return Regex.Replace(value, @"[. ]([a-zA-Z])", m => m.Groups[1].Value.ToUpper());
        }

        public class RenderingItem {
            public bool IsFirst;
            public string JavaScriptValue, CSharpValue;
        }
    }

}
