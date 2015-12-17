using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DevExtreme.AspNet.TagHelpers.Generator {

    static class Utils {

        public static string ToCamelCase(string input) {
            return Char.ToUpper(input[0]) + input.Substring(1);
        }

        public static string ToKebabCase(string input) {
            return Regex.Replace(input, @"([a-z\d])([A-Z])", @"$1-$2").ToLower();
        }

        public static string NormalizeDescription(string text) {
            if(String.IsNullOrWhiteSpace(text))
                return "";

            text = text.Trim();
            text = Regex.Replace(text, @"\s+", " ");
            text = Regex.Replace(text, @"\</?[^>]+\>", ""); // strip tags
            text = Regex.Replace(text, @"_(\w+)_", "'$1'"); // _abc_ -> 'abc'

            if(text.Contains("_"))
                Console.WriteLine(text);

            return text;
        }
    }

}
