using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace DevExtreme.AspNet.TagHelpers.Tests {

    static class AssertIgnoreWhitespaces {

        public static void Equal(string expected, string actual) {
            Assert.Equal(RemoveWhitespace(expected), RemoveWhitespace(actual));
        }

        public static void Contains(string expectedSubstring, string actualString) {
            Assert.Contains(RemoveWhitespace(expectedSubstring), RemoveWhitespace(actualString));
        }

        public static void StartsWith(string expectedStartString, string actualString) {
            Assert.StartsWith(RemoveWhitespace(expectedStartString), RemoveWhitespace(actualString));
        }

        public static void DoesNotMatch(string expectedRegexPattern, string actualString) {
            Assert.DoesNotMatch(expectedRegexPattern, RemoveWhitespace(actualString));
        }

        static string RemoveWhitespace(string text) {
            text = Regex.Replace(text, @"[\r\n]", " ");
            text = Regex.Replace(text, @"(?<=\w)\s+(?=\w)", " ");
            text = Regex.Replace(text, @"(?<=\W)\s+(?=\w)", "");
            text = Regex.Replace(text, @"(?<=\w)\s+(?=\W)", "");
            text = Regex.Replace(text, @"(?<=\W)\s+(?=\W)", "");
            return text;
        }
    }

}
